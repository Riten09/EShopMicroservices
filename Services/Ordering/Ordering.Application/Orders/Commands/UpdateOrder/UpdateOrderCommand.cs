using BuildingBlocks.CQRS;
using Ordering.Application.Dtos;
using System.Windows.Input;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(OrderDto Order)
        :ICommand<UpdateOrderResult>;

    public record UpdateOrderResult(bool IsSuccess);


    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.Order.Id).NotEmpty().WithMessage("OrderId is required.");
            RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required.");
            RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        }
    }
}
