namespace EdwApiDemo.Api.Dto.Auth;

public class AssignRoleRequest
{
    public required int UserId { get; init; }
    public required string Role { get; init; }
}


