using EdwApiDemo.Application.CQRS.Queries.Documents.DTOs;
using EdwApiDemo.Domain.Models;
using EdwApiDemo.Infrastructure.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EdwApiDemo.Application.CQRS.Queries.Documents.Handlers;

public class GetPendingAccessRequestsQueryHandler 
    : IRequestHandler<GetPendingAccessRequestsQuery, IEnumerable<AccessRequestDto>>
{
    private readonly EdwDbContext _context;

    public GetPendingAccessRequestsQueryHandler(EdwDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AccessRequestDto>> Handle(
        GetPendingAccessRequestsQuery request, 
        CancellationToken cancellationToken)
    {
        return await _context.AccessRequests
            .Include(r => r.Document)
            .Include(r => r.RequestedBy)
            .Where(r => r.Status == RequestStatus.Pending)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new AccessRequestDto
            {
                Id = r.Id,
                DocumentId = r.DocumentId,
                DocumentName = r.Document.Name,
                RequestedById = r.RequestedById,
                RequestedByName = r.RequestedBy.Name,
                Status = r.Status,
                AccessType = r.AccessType,
                CreatedAt = r.CreatedAt,
                Comments = r.Comments,
                Decision = null
            })
            .ToListAsync(cancellationToken);
    }
}


