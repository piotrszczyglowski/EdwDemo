using MediatR;
using EdwApiDemo.Domain.Models;
using EdwApiDemo.Domain.Events;
using EdwApiDemo.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EdwApiDemo.Application.CQRS.Commands.Documents.Handlers;

public class CreateAccessRequestCommandHandler : IRequestHandler<CreateAccessRequestCommand, int>
{
    private readonly EdwDbContext _context;
    private readonly IMediator _mediator;

    public CreateAccessRequestCommandHandler(EdwDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<int> Handle(CreateAccessRequestCommand request, CancellationToken cancellationToken)
    {
        var document = await _context.Documents
            .FirstOrDefaultAsync(d => d.Id == request.DocumentId, cancellationToken);

        if (document == null)
            throw new InvalidOperationException("Document not found");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.RequestedById, cancellationToken);

        if (user == null)
            throw new InvalidOperationException("User not found");

        var alreadyRequested = await _context.AccessRequests.FirstOrDefaultAsync(r => r.DocumentId == request.DocumentId && r.RequestedBy == r.RequestedBy);
        if (alreadyRequested != null)
            throw new InvalidOperationException("Document already requested!");

        var accessRequest = new AccessRequest
        {
            DocumentId = request.DocumentId,
            RequestedById = request.RequestedById,
            Status = RequestStatus.Pending,
            Comments = request.Reason,
            AccessType = request.AccessType,
            CreatedAt = DateTime.UtcNow
        };

        _context.AccessRequests.Add(accessRequest);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish domain event
        await _mediator.Publish(new DocumentAccessRequestedEvent
        {
            RequestId = accessRequest.Id,
            DocumentId = request.DocumentId,
            RequestedById = request.RequestedById,
            RequestedByEmail = user.Email,
            AccessType = request.AccessType.ToString(),
            Reason = request.Reason,
            RequestedAt = accessRequest.CreatedAt
        }, cancellationToken);

        return accessRequest.Id;
    }
}


