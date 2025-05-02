using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EdwApiDemo.Application.CQRS.Commands.Documents;
using EdwApiDemo.Application.CQRS.Queries.Documents;
using System.Security.Claims;
using EdwApiDemo.Api.Dto.DocumentAccess;
using EdwApiDemo.Api.Dto;

namespace EdwApiDemo.Api.Controllers;

/// <summary>
/// Controller for managing document access requests and approvals
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
public class DocumentAccessController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DocumentAccessController> _logger;

    /// <summary>
    /// Initializes a new instance of the DocumentAccessController
    /// </summary>
    /// <param name="mediator">The mediator instance for handling commands and queries</param>
    /// <param name="logger">Logger for error handling and diagnostics</param>
    public DocumentAccessController(
        IMediator mediator,
        ILogger<DocumentAccessController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all pending document access requests for an approver
    /// </summary>
    /// <returns>A list of pending access requests</returns>
    /// <response code="200">Returns the list of pending access requests</response>
    /// <response code="400">If there was an error processing the request</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user does not have the Approver role</response>
    [HttpGet("pending")]
    [Authorize(Roles = "Approver")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> GetPendingRequests()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            
            var requests = await _mediator.Send(new GetPendingAccessRequestsQuery 
            { 
            });
            
            return Ok(requests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pending access requests");
            return BadRequest(new ErrorResponse { Error = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new document access request
    /// </summary>
    /// <param name="documentId">Requested document ID</param>
    /// <param name="request">The access request details</param>
    /// <returns>The ID of the newly created request</returns>
    /// <response code="200">Returns the ID of the created request</response>
    /// <response code="400">If there was an error processing the request</response>
    /// <response code="401">If the user is not authenticated</response>
    [HttpPost("request")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> RequestAccess(int documentId, [FromBody] CreateAccessRequestDto request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            
            var requestId = await _mediator.Send(new CreateAccessRequestCommand
            {
                DocumentId = documentId,
                RequestedById = userId,
                Reason = request.Reason,
                AccessType = request.AccessType
            });

            return Ok(new { requestId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating access request");
            return BadRequest(new ErrorResponse { Error = ex.Message });
        }
    }

    /// <summary>
    /// Processes (approves or rejects) a document access request
    /// </summary>
    /// <param name="request">The request processing details</param>
    /// <returns>No content if successful</returns>
    /// <response code="200">If the request was processed successfully</response>
    /// <response code="400">If there was an error processing the request</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user does not have the Approver role</response>
    [HttpPost("process")]
    [Authorize(Roles = "Approver")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> ProcessRequest([FromBody] ProcessAccessRequestDto request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            
            await _mediator.Send(new ProcessAccessRequestCommand
            {
                RequestId = request.RequestId,
                ProcessedById = userId,
                IsApproved = request.IsApproved,
                Justification = request.Reason
            });

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing access request");
            return BadRequest(new ErrorResponse { Error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves details of a specific access request
    /// </summary>
    /// <param name="requestId">The ID of the request to retrieve</param>
    /// <returns>The access request details</returns>
    /// <response code="200">Returns the access request details</response>
    /// <response code="400">If there was an error processing the request</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user does not have the required role</response>
    /// <response code="404">If the request was not found</response>
    [HttpGet("{requestId}")]
    [Authorize(Roles = "Approver, User")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetRequest(int requestId)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var request = await _mediator.Send(new GetAccessRequestQuery { RequestId = requestId, UserId = userId });
            
            if (request == null)
                return NotFound();

            return Ok(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving access request");
            return BadRequest(new ErrorResponse { Error = ex.Message });
        }
    }
}
