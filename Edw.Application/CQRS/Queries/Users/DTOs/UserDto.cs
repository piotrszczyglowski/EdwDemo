namespace EdwApiDemo.Application.CQRS.Queries.Users.DTOs;

public record UserDto
{
    public required int Id { get; init; }
    public required string Email { get; init; }
    public required string Name { get; init; }
}


