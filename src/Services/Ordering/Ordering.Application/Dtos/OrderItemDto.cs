namespace Ordering.Application.Dtos
{
    // This Dto represnts the essential information of each item in an order such as the product id, quantity, and price.   
    public record OrderItemDto
    (
        Guid OrderId,
        Guid ProductId,
        int Quantity,
        decimal Price
        );
}
