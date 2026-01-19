using Application.Abstraction.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Abstraction.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly ILogger<TRequest> _logger;
    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        var name = request.GetType().Name;


        try
        {
            _logger.LogInformation($"Started executing {name}");
            var response = await next();
            _logger.LogInformation($"Finished executing {name}");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while executing {name}");

            throw;
        }


    }
}
