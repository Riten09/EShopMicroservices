

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto):
        ICommand<CheckoutBasketResult>;
    public record CheckoutBasketResult(bool IsSuccess);

    public class BasketCheckoutCommandValidation : AbstractValidator<CheckoutBasketCommand>
    {
        public BasketCheckoutCommandValidation()
        {
            RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto is required");
            RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("Username is required");
        }
    }
    public class CheckoutBasketCommandHandler
        (IBasketRepository repository, IPublishEndpoint publishEndpoint)
        : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            //get existing basket with total price
            // Set totalprice on basketcheckout event message
            // send basket checkout event to RabbitMQ using MassTransit
            // delete the basket

            var basket = await repository.GetBasket(command.BasketCheckoutDto.UserName, cancellationToken);
            if(basket is null)
            {
                return new CheckoutBasketResult(false);
            }

            var eventMessage = command.BasketCheckoutDto.Adapt<BaskerCheckoutEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;

            await publishEndpoint.Publish(eventMessage, cancellationToken);
            await repository.DeleteBasket(command.BasketCheckoutDto.UserName, cancellationToken);

            return new CheckoutBasketResult(true);

        }
    }
}
