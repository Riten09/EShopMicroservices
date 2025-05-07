using Catalog.API.Products.GetProducts;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections;

namespace Catalog.API.Products.GetProductByCategory
{
    //public record GetProductByCategoryRequest();

    public record GetProductByIDCategoryResponse(IEnumerable<Product> Products);
    public class GetProductByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async(string category, ISender sender)=>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));

                var response = result.Adapt<GetProductByCategoryResult>();

                
                return Results.Ok(response);
            }).WithName("GetProductsBycategory")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products By category")
            .WithDescription("Get Products By category");
        }
    }
}
