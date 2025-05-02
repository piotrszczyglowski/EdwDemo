using MediatR;
using EdwApiDemo.Application.CQRS.Queries.Documents.DTOs;

namespace EdwApiDemo.Application.CQRS.Queries.Documents;

public record GetAccessRequestQuery : IRequest<AccessRequestDto?>
{
    public required int RequestId { get; init; }
    public required int UserId { get; init; }
}


