using System.Net;

namespace CandidateHubApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ctx, ex);
            }
        }

        //TODO: log the exception to file or db
        private Task HandleExceptionAsync(HttpContext ctx, Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred!");

            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = new
            {
                ctx.Response.StatusCode,
                Message = "Internal Server Error, Please try again later"
            };
            return ctx.Response.WriteAsJsonAsync(response);
        }
    }
}
