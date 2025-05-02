using EdwApiDemo.Domain.Events;

namespace EdwApiDemo.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync(IEvent @event);
    IEnumerable<IEvent> DequeueEvents();
}
