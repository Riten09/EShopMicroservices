using MediatR;

namespace BuildingBlocks.CQRS
{
    // this interface will habdle our query request, match each query with correponding query type
    public interface IQueryHandler<in TQuery, TResponse>
        : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : notnull
    {
    }
}
