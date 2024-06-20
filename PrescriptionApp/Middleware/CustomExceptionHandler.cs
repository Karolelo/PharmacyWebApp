using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using PrescriptionApp.Exceptions;

namespace PrescriptionApp.Middleware
{
    public class CustomExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(RequestDelegate next, ILogger<CustomExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            switch (exception)
            {
                case PrescriptionException presException:
                case PatientException patientException:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                default:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            return context.Response.WriteAsync(new
            {
                StatusCode = response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
    }
}