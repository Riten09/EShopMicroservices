using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse> 
        (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Hanlde request={Request} - Response={Response} - RequestData={RequestData}",
                typeof(TRequest).Name, typeof(TResponse).Name, request);

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();
            var timetaken = timer.Elapsed;
            if(timetaken.Seconds > 3) // if the request is greater than 3 second, then log performance warning log
            {
                logger.LogWarning("[PERFORMANCE] The request {Request} took {TIMETAKEN}",
                    typeof(TRequest).Name, timetaken.Seconds);
            }
            logger.LogInformation("[END] Hanlde {Request} with {Response}",
                typeof(TRequest).Name,typeof(TResponse).Name);

            return response;

        }
    }
}
