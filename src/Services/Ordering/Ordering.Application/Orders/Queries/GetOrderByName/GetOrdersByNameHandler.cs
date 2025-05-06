
namespace Ordering.Application.Orders.Queries.GetOrderByName
{
    public record GetOrdersByNameHandler(IApplicationDbContext DbContext)
        : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {
            //get order by name using dbcontext
            // return result
            var orders = await DbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.OrderName.Value.Contains(query.Name))
                .OrderBy(o => o.OrderName.Value)
                .ToListAsync(cancellationToken);
            return new GetOrdersByNameResult(orders.ToOrderDtoList());
        }

        
    }
}

