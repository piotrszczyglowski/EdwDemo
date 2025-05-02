using MediatR;
using EdwApiDemo.Domain.Models;
using EdwApiDemo.Domain.Events;
using EdwApiDemo.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace EdwApiDemo.Application.CQRS.Commands.Documents.Handlers;

public class ProcessAccessRequestCommandHandler : IRequestHandler<ProcessAccessRequestCommand, Unit>
{
    private readonly EdwDbContext _context;
    private readonly IMediator _mediator;

    public ProcessAccessRequestCommandHandler(EdwDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ProcessAccessRequestCommand request, CancellationToken cancellationToken)
    {
        var accessRequest = await _context.AccessRequests
            .Include(r => r.Document)
            .FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);

        if (accessRequest == null)
            throw new InvalidOperationException("Access request not found");

        if (!request.IsApproved && string.IsNullOrWhiteSpace(request.Justification))
            throw new InvalidOperationException("Justification is required when rejecting request");

        var decision = new Decision
        {
            AccessRequestId = request.RequestId,
            DecidedById = request.ProcessedById,
            IsApproved = request.IsApproved,
            Justification = request.Justification,
            CreatedAt = DateTime.UtcNow
        };

        _context.Decisions.Add(decision);
        accessRequest.Status = request.IsApproved ? RequestStatus.Approved : RequestStatus.Rejected;
        accessRequest.DecisionId = decision.Id;

        await _context.SaveChangesAsync(cancellationToken);

        // Publish domain event
        await _mediator.Publish(new DocumentAccessProcessedEvent
        {
            RequestId = accessRequest.Id,
            DocumentId = accessRequest.DocumentId,
            ProcessedById = request.ProcessedById,
            IsApproved = request.IsApproved,
            Justification = request.Justification,
            ProcessedAt = decision.CreatedAt
        }, cancellationToken);

        return Unit.Value;
    }
}


