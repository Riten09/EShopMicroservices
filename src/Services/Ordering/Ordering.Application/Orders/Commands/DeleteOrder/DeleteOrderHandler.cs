
namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public record DeleteOrderHandler(IApplicationDbContext dbContext)
        : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            // delete Order entity from command object
            // save to database
            // return result

            var orderId = OrderId.Of(command.orderId);
            var order = await dbContext.Orders.FindAsync([orderId], cancellationToken: cancellationToken);
            if (order is null)
            {
                throw new OrderNotFoundException(command.orderId);
            }

            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new DeleteOrderResult(true);

        }
    }
}
