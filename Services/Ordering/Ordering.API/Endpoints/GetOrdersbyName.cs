
using Ordering.Application.Orders.Queries.GetOrderByName;

namespace Ordering.API.Endpoints
{
    //- Accepts a name parameter.
    //- Constructs a GetOrderByNameQuery with these parameter.
    //- Retrieves and returns matching orders.

    //public record GetOrdersByNameRequest(string Name);
    public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);
    public class GetOrdersbyName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{orderName}", async(string orderName, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByNameQuery(orderName));

                var response = result.Adapt<GetOrdersByNameResponse>();

                return Results.Ok(response);
            })
             .WithName("GetOrdersByName")
             .Produces<GetOrdersByNameResponse>(StatusCodes.Status201Created)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .WithSummary("Get Orders By Name")
             .WithDescription("Get Orders By Name");
        }
    }
}
