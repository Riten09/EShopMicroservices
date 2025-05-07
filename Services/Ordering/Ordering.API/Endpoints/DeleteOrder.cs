
using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints
{
    //- Accepts a order ID as a parameter.
    //- Constructs a DeleteOrderCommand.
    //- Send the command using MediatR.
    //- Returs a success or not found response.

    //public record DeleteOrderRequest(Guid Id);
    public record DeleteOrderResponse(bool IsSuccess);
    public class DeleteOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/orders/{id}", async (Guid Id, ISender sender) =>
            {

                var result = await sender.Send(new DeleteOrderCommand(Id));

                var response = result.Adapt<DeleteOrderResponse>();

                return Results.Ok(response);
            })
             .WithName("DeleteOrder")
             .Produces<DeleteOrderResponse>(StatusCodes.Status201Created)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Delete Order")
             .WithDescription("Create Order");
        }
    }
}
