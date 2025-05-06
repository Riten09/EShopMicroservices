
namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string UserName): ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsSuccess);

    public class DeleteBasketCommandValidation: AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
        }
    }
    public class DeleteBasketCommandHandle(IBasketRepository repository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            // TODO: delete basket from database
            // TODO: session.Delete<Product>(command.Id)

            await repository.DeleteBasket(command.UserName, cancellationToken);

            return new DeleteBasketResult(true);
        }
    }
}
