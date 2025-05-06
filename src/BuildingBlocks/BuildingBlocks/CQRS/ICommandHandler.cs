using MediatR;

namespace BuildingBlocks.CQRS
{
    // interface create where response can be null no response result 
    public interface ICommandHandler<in TCommand>
        : ICommandHandler<TCommand, Unit>
        where TCommand : ICommand<Unit>
    {

    }

    // interface create where response will not be null, with response result
    public interface ICommandHandler<in TCommand, TResponse>
        :IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse :notnull
    {
    }
}
