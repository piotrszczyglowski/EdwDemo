using MediatR;
using EdwApiDemo.Api.Dto.Auth;
using EdwApiDemo.Application.CQRS.Commands.Users;
using EdwApiDemo.Application.CQRS.Queries.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EdwApiDemo.Application.CQRS.Queries.Users.DTOs;
using EdwApiDemo.Api.Dto;

namespace EdwApiDemo.Api.Controllers;

/// <summary>
/// Controller for authentication and user registration
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the AuthController
    /// </summary>
    public AuthController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="request">User registration details</param>
    /// <returns>The ID of the newly created user</returns>
    /// <response code="200">Returns the ID of the created user</response>
    /// <response code="400">If there was an error processing the request</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Register(RegisterRequest request)
    {
        try
        {
            var command = new CreateUserCommand
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password
            };

            var userId = await _mediator.Send(command);

            return Ok(new { id = userId });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse { Error = ex.Message });
        }
    }

    /// <summary>
    /// ONLY FOR TESTING PURPOSES Authenticates a user and returns a JWT token
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT authentication token</returns>
    /// <response code="200">Returns the JWT token</response>
    /// <response code="400">If there was an error processing the request</response>
    /// <response code="401">If authentication failed</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var query = new GetUserByEmailQuery { Email = request.Email };
        var user = await _mediator.Send(query);

        if (user == null)
            return Unauthorized();

        // Password verification should be moved to a separate command
        var rolesQuery = new GetUserRolesQuery { UserId = user.Id };
        var roles = await _mediator.Send(rolesQuery);

        var token = GenerateJwtToken(user, roles);

        return Ok(new { token });
    }

    private string GenerateJwtToken(UserDto user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

