using IncidentManagement.API.Infrastructure.Data;
using IncidentManagement.API.Domain.Entities;
using System.Security.Claims;

namespace IncidentManagement.API.Middleware;

public class AuditMiddleware
{
    private readonly RequestDelegate _next;

    public AuditMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext db)
    {
        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        var username =
            context.User?.Identity?.IsAuthenticated == true
            ? context.User.FindFirst(ClaimTypes.Name)?.Value
            : "Anonymous";

        var path = context.Request.Path;
        var method = context.Request.Method;

        await _next(context);

        responseBody.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();

        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);

        string? errorMessage = null;

        if (context.Response.StatusCode >= 400)
        {
            errorMessage = responseText;
        }

        if (username != "Anonymous")
        {
            var audit = new AuditLog
            {
                Id = Guid.NewGuid(),
                Action = $"{method} {path}",
                PerformedBy = username!,
                TargetUser = "-",
                OldRole = "-",
                NewRole = "-",
                CreatedAt = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                StatusCode = context.Response.StatusCode,
                Success = context.Response.StatusCode < 400,
                ErrorMessage = errorMessage
            };

            db.AuditLogs.Add(audit);
            await db.SaveChangesAsync();
        }
    }
}