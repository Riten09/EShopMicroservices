
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Id);

    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async(Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(id));

                var response = result.Adapt<DeleteProductResponse>();

                return Results.Ok(response);
            })
                .WithName("DeleteProduct") //returning as http post method name
                .Produces<DeleteProductResponse>(StatusCodes.Status200OK) // return with success status code
                .ProducesProblem(StatusCodes.Status400BadRequest) // if there is any problem
                .ProducesProblem(StatusCodes.Status404NotFound) // if there is any problem
                .WithSummary("Delete Product")
                .WithDescription("Delete Product");
        }
    }
}
