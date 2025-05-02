using MediatR;
using EdwApiDemo.Api.Dto.Auth;
using EdwApiDemo.Application.CQRS.Commands.Users;
using EdwApiDemo.Application.CQRS.Queries.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EdwApiDemo.Api.Controllers;

[ApiController]
[Route("api/users/roles")]
[Authorize(Roles = "Admin")]
public class UserRolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserRolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("assign")]
    public async Task<ActionResult> AssignRole(AssignRoleRequest request)
    {
        try
        {
            var command = new AssignRoleCommand
            {
                UserId = request.UserId,
                Role = request.Role
            };

            await _mediator.Send(command);
            return Ok(new { message = "Role assigned successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(int userId)
    {
        try
        {
            var query = new GetUserRolesQuery { UserId = userId };
            var roles = await _mediator.Send(query);
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}


