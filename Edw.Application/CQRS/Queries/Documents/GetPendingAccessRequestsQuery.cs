using MediatR;
using EdwApiDemo.Application.CQRS.Queries.Documents.DTOs;

namespace EdwApiDemo.Application.CQRS.Queries.Documents;

public record GetPendingAccessRequestsQuery : IRequest<IEnumerable<AccessRequestDto>>
{
}


