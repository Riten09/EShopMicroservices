using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviours
{
    /// Summary <summary>
    /// Class ValidationBehaviour will take Trequest as request & return TResponse as response
    /// IPipelineBehaviour will give request to process and then next logic for implementation 
    /// Where TRequest, we add a filet for validation behaviour  which basically is TRequest but we have changed it to ICommand that mean whatever Trequest will come
    /// Command which meand CRUD operations.
    /// IEnumberable & Ivalidatior represent the Ivalidator which means when we apply Ivalidator in our command handler it will only apply for that specific handler 
    /// in order to call all the handler method we are using this, so we can validate it in place
    /// </summary>
    public class ValidationBehaviour<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults =
                await Task.WhenAll(validators.Select(v=>v.ValidateAsync(context, cancellationToken)));

            var failure =
                validationResults.Where(r=>r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if(failure.Any())
            {
                throw new ValidationException(failure);
            }

            return await next();

        }
    }
}
