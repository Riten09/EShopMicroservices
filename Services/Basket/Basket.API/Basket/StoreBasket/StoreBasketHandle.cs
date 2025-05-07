
using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidatior : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidatior()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    public class StoreBasketCommandHandle(IBasketRepository repository,DiscountProtoService.DiscountProtoServiceClient discountProto) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            // call to DeductDiscount method  to get the discount from gRPC service
            await DeductDiscount(command.Cart, cancellationToken); 

            // TODO: store basket to database(user Marten upsert - if exist = update, if no = insert
            // TODO: update cache
            await repository.StoreBasket(command.Cart, cancellationToken);

            return new StoreBasketResult(command.Cart.UserName);
        }

        // calling discount gRPC service to get the discount on the product 
        public async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest {ProductName = item.ProductName}, cancellationToken:cancellationToken);
                item.Price -= coupon.Amount;
            }
        }
    }

    
}
