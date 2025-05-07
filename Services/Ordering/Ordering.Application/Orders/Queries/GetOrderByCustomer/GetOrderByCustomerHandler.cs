

namespace Ordering.Application.Orders.Queries.GetOrderByCustomer
{
    public record GetOrderByCustomerHandler(IApplicationDbContext DbContext)
        : IQueryHandler<GetOrderByCustomerQuery, GetOrderByCustomerResult>
    {
        public async Task<GetOrderByCustomerResult> Handle(GetOrderByCustomerQuery query, CancellationToken cancellationToken)
        {
            //get order by customer using dbcontext
            // return result

            var orders = await DbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.CustomerId == CustomerId.Of(query.CustomerID)) // coverting CustomerID using Of method with incoming query object and that will provide to covert strongly type CustomerOD when filtering the orders by CustomerID
                .OrderBy(o => o.OrderName.Value)
                .ToListAsync(cancellationToken);

            return new GetOrderByCustomerResult(orders.ToOrderDtoList());
        }
    }
}
