using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace LogiTrack.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");

            await HandleExceptionAsync(context);
        }
    }

    private static async Task HandleValidationExceptionAsync(
        HttpContext context,
        ValidationException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/problem+json";

        var errors = exception.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(x => x.ErrorMessage).ToArray());

        var problem = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed.",
            Type = "https://httpstatuses.com/400"
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problem));
    }

    private static async Task HandleExceptionAsync(
        HttpContext context)
    {
        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred.",
            Type = "https://httpstatuses.com/500"
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problem));
    }
}