using MediatR;

namespace EdwApiDemo.Application.CQRS.Commands.Users;

public record CreateUserCommand : IRequest<int>
{
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required string Password { get; init; }
}


