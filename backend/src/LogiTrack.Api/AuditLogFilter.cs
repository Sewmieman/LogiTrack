using Microsoft.AspNetCore.Mvc.Filters;

namespace LogiTrack.Api;

public sealed class AuditLogFilter : IAsyncActionFilter
{
    private readonly ILogger<AuditLogFilter> _logger;

    public AuditLogFilter(ILogger<AuditLogFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var user = context.HttpContext.User.Identity?.Name ?? "anonymous";
        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;

        _logger.LogInformation(
            "AUDIT START: {Method} {Path} by {User}",
            method, path, user);

        var result = await next();

        _logger.LogInformation(
            "AUDIT END: {Method} {Path} by {User}; status={StatusCode}",
            method, path, user,
            context.HttpContext.Response.StatusCode);
    }
}
