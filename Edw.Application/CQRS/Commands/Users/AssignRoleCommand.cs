using MediatR;

namespace EdwApiDemo.Application.CQRS.Commands.Users;

public record AssignRoleCommand : IRequest<Unit>
{
    public required int UserId { get; init; }
    public required string Role { get; init; }
}


