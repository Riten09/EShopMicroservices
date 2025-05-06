using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderCreatedEventHandler(IPublishEndpoint publishEndpoint,IFeatureManager featureManager,ILogger<OrderCreatedEventHandler> logger)
        : INotificationHandler<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

            //adding FetaureManagement flag
            if(await featureManager.IsEnabledAsync("OrderFullfilment"))
            {

                var orderCreatedIntegrationEvent = domainEvent.order.ToOderDto();

                await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
            }

        }
    }
}
