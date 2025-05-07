

namespace Catalog.API.Products.GetProducts
{
    //create query to get the products
    public record GetProductsQuery(int PageNumber = 1, int PageSize = 10) :IQuery<GetProductsResult>;

    //create member to to get the products as result
    public record GetProductsResult(IEnumerable<Product> Products);
    internal class GetProductsQueryHandler (IDocumentSession session)
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {

            var products = await session.Query<Product>().ToPagedListAsync(query.PageNumber, query.PageSize , cancellationToken);

            return new GetProductsResult(products); 
        }
    }
}
