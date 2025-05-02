using MediatR;
using EdwApiDemo.Application.CQRS.Queries.Users.DTOs;

namespace EdwApiDemo.Application.CQRS.Queries.Users;

public record GetUserByEmailQuery : IRequest<UserDto?>
{
    public required string Email { get; init; }
}


