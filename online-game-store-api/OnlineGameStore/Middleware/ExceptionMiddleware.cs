using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlineGameStore.BLL.CustomExceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;


namespace OnlineGameStore.Middleware
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

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message;
            switch (exception.GetType().Name)
            {
                case nameof(NotFoundException):
                    message = "Not found";
                    status = HttpStatusCode.NotFound;
                    break;
                case nameof(ModelException):
                    message = exception.Message;
                    status = HttpStatusCode.InternalServerError;
                    break;
                case nameof(ForrbidenException):
                    message = exception.Message;
                    status = HttpStatusCode.Forbidden;
                    break;
                default:
                    {
                        message = "Error occurred";
                        status = HttpStatusCode.InternalServerError;
                        _logger.LogError(exception, exception.Message, new
                        {
                            exception.StackTrace,
                            exception.Source,
                            exception.InnerException,
                            exception.TargetSite
                        });
                        break;
                    }
            }

            string result = JsonSerializer.Serialize(new
            {
                message,
                status,
                errorDateTime = DateTime.UtcNow
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(result);
        }

    }
}
