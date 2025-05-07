namespace Ordering.Application.Orders.Queries.GetOrderByCustomer
{
    public record GetOrderByCustomerQuery(Guid CustomerID)
        :IQuery<GetOrderByCustomerResult>;

    public record GetOrderByCustomerResult(IEnumerable<OrderDto> Orders);
}
