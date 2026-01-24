using Serilog.Context;

namespace API.Middleware
{
    public class RequestContextLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public const string CorrelationIdHeaderName = "X-Correlation-ID";
        public RequestContextLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("CorrelationId", GetCorreltaionId(context)))
            {

                return _next(context);
            }
        }

        private static string GetCorreltaionId(HttpContext context)
        {
            context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);

            return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
        }

    }
}
