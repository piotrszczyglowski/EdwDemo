using MediatR;

namespace EdwApiDemo.Domain.Events;

public interface IEvent : INotification
{
    DateTime Timestamp { get; }
}

public record DocumentAccessRequestedEvent : IEvent
{
    public int RequestId { get; init; }
    public int DocumentId { get; init; }
    public int RequestedById { get; init; }
    public string RequestedByEmail { get; init; }
    public string AccessType { get; init; }
    public string Reason { get; init; }
    public DateTime RequestedAt { get; init; }
    public DateTime Timestamp => RequestedAt;
}

public record DocumentAccessProcessedEvent : IEvent
{
    public int RequestId { get; init; }
    public int DocumentId { get; init; }
    public int ProcessedById { get; init; }
    public bool IsApproved { get; init; }
    public string Justification { get; init; }
    public DateTime ProcessedAt { get; init; }
    public DateTime Timestamp => ProcessedAt;
}
