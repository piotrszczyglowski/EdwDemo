using MediatR;

namespace EdwApiDemo.Application.CQRS.Queries.Users;

public record GetUserRolesQuery : IRequest<IEnumerable<string>>
{
    public required int UserId { get; init; }
}


