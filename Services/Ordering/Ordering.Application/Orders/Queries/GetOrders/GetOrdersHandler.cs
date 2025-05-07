
using BuildingBlocks.Pagination;

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public record GetOrdersHandler(IApplicationDbContext DbContext)
        : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            //get orders with pagination
            // return result

            var pageIndex = query.PaginatedRequest.PageIndex;
            var pageSize = query.PaginatedRequest.PageSize;

            //Will return  total count of our order records on database
            var totalCount = await DbContext.Orders.LongCountAsync(cancellationToken);

            var orders = await DbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .OrderBy(o => o.OrderName.Value)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new GetOrdersResult(
                new PaginatedResult<OrderDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    orders.ToOrderDtoList()));
        }
    }
}
