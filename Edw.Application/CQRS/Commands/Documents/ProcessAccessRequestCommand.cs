using MediatR;

namespace EdwApiDemo.Application.CQRS.Commands.Documents;

public record ProcessAccessRequestCommand : IRequest<Unit>
{
    public required int RequestId { get; init; }
    public required int ProcessedById { get; init; }
    public required bool IsApproved { get; init; }
    public string? Justification { get; init; }
}


