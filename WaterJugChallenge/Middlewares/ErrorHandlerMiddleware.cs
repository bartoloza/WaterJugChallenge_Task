using AppModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace WebApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
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

                var result = JsonSerializer.Serialize(problemDetails);
                await response.WriteAsync(result);
            }
        }

        private ProblemDetails CreateProblemDetails(Exception error, string requestPath, string traceId)
        {
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

            switch (error)
            {
                case ArgumentNullException _:
                case ArgumentException _:
                case FormatException _:
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Invalid Parameters";
                    problemDetails.Detail = "One or more required parameters are missing or invalid.";
                    break;
                case AppException e:
                    // custom application error
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "No Solution";
                    break;

                default:
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return problemDetails;
        }
    }
}
