using EdwApiDemo.Domain.Models;
using MediatR;

namespace EdwApiDemo.Application.CQRS.Commands.Documents;

public record CreateAccessRequestCommand : IRequest<int>
{
    public required int DocumentId { get; init; }
    public required int RequestedById { get; init; }
    public required string Reason { get; init; }
    public required AccessType AccessType { get; init; }
}


