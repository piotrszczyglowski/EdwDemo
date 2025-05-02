using EdwApiDemo.Domain.Events;
using EdwApiDemo.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MediatR;

namespace EdwApiDemo.Infrastructure.Services;

public class NotificationBackgroundService : BackgroundService
{
    private readonly ILogger<NotificationBackgroundService> _logger;
    private readonly IEventPublisher _eventPublisher;
    private readonly IMediator _mediator;

    public NotificationBackgroundService(
        ILogger<NotificationBackgroundService> logger,
        IEventPublisher eventPublisher,
        IMediator mediator)
    {
        _logger = logger;
        _eventPublisher = eventPublisher;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Process events from the queue
                var events = _eventPublisher.DequeueEvents();
                foreach (var @event in events)
                {
                    // First publish to MediatR for any application handlers
                    await _mediator.Publish(@event, stoppingToken);
                    
                    // Then process for notifications
                    await ProcessEventAsync(@event);
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing notification events");
            }
        }
    }

    private Task ProcessEventAsync(IEvent @event)
    {
        switch (@event)
        {
            case DocumentAccessRequestedEvent requestEvent:
                return SendAccessRequestNotificationAsync(requestEvent);
            case DocumentAccessProcessedEvent processedEvent:
                return SendProcessedRequestNotificationAsync(processedEvent);
            default:
                return Task.CompletedTask;
        }
    }

    private Task SendAccessRequestNotificationAsync(DocumentAccessRequestedEvent @event)
    {
        _logger.LogInformation(
            "New access request: Document {DocumentId} requested by {RequestedBy} for {AccessType} access. Reason: {Reason}",
            @event.DocumentId,
            @event.RequestedByEmail,
            @event.AccessType,
            @event.Reason);

        // TODO: Implement actual notification sending 
        return Task.CompletedTask;
    }

    private Task SendProcessedRequestNotificationAsync(DocumentAccessProcessedEvent @event)
    {
        _logger.LogInformation(
            "Access request {RequestId} for document {DocumentId} was {Status}. Justification: {Justification}",
            @event.RequestId,
            @event.DocumentId,
            @event.IsApproved ? "approved" : "rejected",
            @event.Justification);

        // TODO: Implement actual notification sending 
        return Task.CompletedTask;
    }
}
