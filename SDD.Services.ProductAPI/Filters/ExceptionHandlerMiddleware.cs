﻿using SDD.Services.ProductAPI.Models.Dto;
using System.Text.Json;

namespace SDD.Services.ProductAPI.Filters
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    await HandleException(httpContext, "You are forbidden from accessing this resource.", StatusCodes.Status403Forbidden);
                }
                else if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await HandleException(httpContext, "You are not authorized to access this resource.", StatusCodes.Status401Unauthorized);
                }
                
                else if (httpContext.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    await HandleException(httpContext, "You have made too many requests in such a small time frame", StatusCodes.Status429TooManyRequests);
                }

            }
            catch (Exception error)
            {
                _logger.LogError(error, "An error occurred.");
                await HandleException(httpContext, "An error occurred, please try again later!", StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task HandleException(HttpContext httpContext, string message, int statusCode)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = statusCode;

            var responseModel = new ResponseDto { Message = message, IsSuccess = false };
            var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await response.WriteAsync(result);
        }
    }
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
