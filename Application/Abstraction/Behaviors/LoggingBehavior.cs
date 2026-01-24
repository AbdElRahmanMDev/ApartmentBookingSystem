using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Application.Abstraction.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        var name = request.GetType().Name;


        try
        {
            _logger.LogInformation("Started executing {Request}", name);
            var response = await next();

            if (response.IsSuccess)
            {
                _logger.LogInformation("Finished executing {Request}", name);

            }
            else
            {
                using (LogContext.PushProperty("Error", response.Error, true))
                {
                    _logger.LogError("Request {Request} processed with error ", name);

                }

            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while executing {name}", name);

            throw;
        }


    }
}
