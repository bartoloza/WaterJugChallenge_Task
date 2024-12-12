using AppModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace WebApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var traceId = context.TraceIdentifier;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var problemDetails = CreateProblemDetails(ex, context.Request.Path, traceId);

                response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

                _logger.LogError(ex, "Error occurred while processing the request: {TraceId}", traceId);

                // Return the ProblemDetails response
                var result = JsonSerializer.Serialize(problemDetails);
                await response.WriteAsync(result);
            }
        }

        private ProblemDetails CreateProblemDetails(Exception error, string requestPath, string traceId)
        {
            // Create a ProblemDetails object with default values
            var problemDetails = new ProblemDetails
            {
                Type = "https://httpstatuses.com/" + (int)HttpStatusCode.InternalServerError,
                Title = "An unexpected error occurred.",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = error.Message,
                Instance = requestPath,
                Extensions =
                {
                    { "traceId", traceId }
                }
            };

            // Customize ProblemDetails based on the exception type
            switch (error)
            {
                case AppException appEx:
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Bad Request";
                    problemDetails.Detail = appEx.Message;
                    break;
                case KeyNotFoundException _:
                    problemDetails.Status = (int)HttpStatusCode.NotFound;
                    problemDetails.Title = "Resource Not Found";
                    break;
                case UnauthorizedAccessException _:
                    problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Title = "Unauthorized";
                    break;
                default:
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return problemDetails;
        }
    }
}
