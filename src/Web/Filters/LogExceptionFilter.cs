using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Web.Filters
{
    /// <summary>
    /// Writes log message for every unhandled error that happens in MVC
    /// </summary>
    public class LogExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public LogExceptionFilter(ILogger<LogExceptionFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void OnException(ExceptionContext context) => this.log(context);

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            this.log(context);
            return Task.CompletedTask;
        }

        private void log(ExceptionContext context)
        {
            _logger.LogError(
                context.Exception,
                "An unhandled exception has occurred {@request}",
                new
                {
                    Path = context.HttpContext?.Request.Path,
                    Query = context.HttpContext?.Request.QueryString,
                    User = context.HttpContext?.User?.Identity?.Name ?? "anonymous",
                    Address = context.HttpContext?.Request?.HttpContext.Connection.RemoteIpAddress?.ToString()
                });
        }
    }
}
