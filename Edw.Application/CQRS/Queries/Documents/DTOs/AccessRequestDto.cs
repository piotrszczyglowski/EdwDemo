using EdwApiDemo.Domain.Models;

namespace EdwApiDemo.Application.CQRS.Queries.Documents.DTOs;

public record AccessRequestDto
{
    public required int Id { get; init; }
    public required int DocumentId { get; init; }
    public required string DocumentName { get; init; }
    public required int RequestedById { get; init; }
    public required string RequestedByName { get; init; }
    public required RequestStatus Status { get; init; }
    public required AccessType AccessType { get; init; }
    public required DateTime CreatedAt { get; init; }
    public string? Comments { get; init; }
    public DecisionDto? Decision { get; init; }
}

public record DecisionDto
{
    public required int Id { get; init; }
    public required int DecidedById { get; init; }
    public required string DecidedByName { get; init; }
    public required bool IsApproved { get; init; }
    public string? Justification { get; init; }
    public required DateTime CreatedAt { get; init; }
}


