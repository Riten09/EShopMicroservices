using BuildingBlocks.Messaging.Events;
using MassTransit;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.EventHandlers.Integration
{
    public class BasketCheckoutEventHandler 
        (ISender sender, ILogger<BasketCheckoutEventHandler> logger)
        : IConsumer<BaskerCheckoutEvent>
    {
        public async Task Consume(ConsumeContext<BaskerCheckoutEvent> context)
        {
            //TODO: Create new order and start order fullfillment process

            logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

            var command = MapToCreateOrderCommand(context.Message);
            await sender.Send(command);
        }

        private CreateOrderCommand MapToCreateOrderCommand(BaskerCheckoutEvent message)
        {
            //Create full order with incoming event data
            var addressDto = new AddressDto( message.FirstName, message.LastName,message.EmailAddress,message.AddressLine,message.Country,message.State,message.ZipCode);
            var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV, message.PaymentMethod);
            var orderId = Guid.NewGuid();

            var orderDto = new OrderDto(
                Id:orderId,
                CustomerId: message.CustomerId,
                OrderName: message.UserName,
                ShippingAddress: addressDto,
                BillingAddress:addressDto,
                Payment: paymentDto,
                Status: Ordering.Domain.Enums.OrderStatus.Pending,
                OrderItems:
                [
                    new OrderItemDto(orderId, new Guid("bdd65744-1795-44a5-a83b-07da8552ad4c"),2,500),
                    new OrderItemDto(orderId, new Guid("9a5698f0-c2ff-454f-a0d3-1636d2c03a91"), 1, 500)
                ]);

            return new CreateOrderCommand(orderDto);
        }
    }

}
