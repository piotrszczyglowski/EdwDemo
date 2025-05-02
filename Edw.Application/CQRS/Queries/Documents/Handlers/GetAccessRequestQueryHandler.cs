using MediatR;
using EdwApiDemo.Application.CQRS.Queries.Documents.DTOs;
using EdwApiDemo.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EdwApiDemo.Application.CQRS.Queries.Documents.Handlers;

public class GetAccessRequestQueryHandler : IRequestHandler<GetAccessRequestQuery, AccessRequestDto?>
{
    private readonly EdwDbContext _context;

    public GetAccessRequestQueryHandler(EdwDbContext context)
    {
        _context = context;
    }

    public async Task<AccessRequestDto?> Handle(GetAccessRequestQuery request, CancellationToken cancellationToken)
    {
        var accessRequest = await _context.AccessRequests
            .Include(r => r.Document)
            .Include(r => r.RequestedBy)
            .Include(r => r.Decision)
                .ThenInclude(d => d.DecidedBy)
            .FirstOrDefaultAsync(r => r.Id == request.RequestId && r.RequestedById == request.UserId, cancellationToken);

        if (accessRequest == null)
            return null;

        return new AccessRequestDto
        {
            Id = accessRequest.Id,
            DocumentId = accessRequest.DocumentId,
            DocumentName = accessRequest.Document.Name,
            RequestedById = accessRequest.RequestedById,
            RequestedByName = accessRequest.RequestedBy.Name,
            Status = accessRequest.Status,
            AccessType = accessRequest.AccessType,
            CreatedAt = accessRequest.CreatedAt,
            Comments = accessRequest.Comments,
            Decision = accessRequest.Decision == null ? null : new DecisionDto
            {
                Id = accessRequest.Decision.Id,
                DecidedById = accessRequest.Decision.DecidedById,
                DecidedByName = accessRequest.Decision.DecidedBy.Name,
                IsApproved = accessRequest.Decision.IsApproved,
                Justification = accessRequest.Decision.Justification,
                CreatedAt = accessRequest.Decision.CreatedAt
            }
        };
    }
}


