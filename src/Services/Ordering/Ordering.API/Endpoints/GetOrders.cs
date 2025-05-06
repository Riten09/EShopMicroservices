using BuildingBlocks.Pagination;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.Endpoints
{
    //- Accepts pagination parameter.
    //- Constructs a GetOrderQuery with these parameters.
    //- Retrieves the data and returns it in a ppaginated format.

    //public record GetOrdersRequest(PaginatedRequest PaginatedRequest);
    public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);
    public class GetOrders : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders", async([AsParameters]PaginatedRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersQuery(request));

                var response = result.Adapt<GetOrdersResponse>();

                return Results.Ok(response);
            })
             .WithName("GetOrders")
             .Produces<GetOrdersResponse>(StatusCodes.Status201Created)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .WithSummary("Get Orders")
             .WithDescription("Get Orders");
        }
    }
}
