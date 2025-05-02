using System.Collections.Concurrent;
using EdwApiDemo.Domain.Events;
using EdwApiDemo.Domain.Interfaces;

namespace EdwApiDemo.Infrastructure.Services;

public class EventPublisher : IEventPublisher
{
    private readonly ConcurrentQueue<IEvent> _eventQueue = new();

    public Task PublishAsync(IEvent @event)
    {
        _eventQueue.Enqueue(@event);
        return Task.CompletedTask;
    }

    public IEnumerable<IEvent> DequeueEvents()
    {
        var events = new List<IEvent>();
        while (_eventQueue.TryDequeue(out var @event))
        {
            events.Add(@event);
        }
        return events;
    }
}
