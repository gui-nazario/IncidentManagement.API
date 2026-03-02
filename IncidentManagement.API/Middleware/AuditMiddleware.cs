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
        var username =
            context.User?.Identity?.IsAuthenticated == true
            ? context.User.FindFirst(ClaimTypes.Name)?.Value
            : "Anonymous";

        var path = context.Request.Path;
        var method = context.Request.Method;

        string? errorMessage = null;

        try
        {
            // Executa pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            throw;
        }
        finally
        {
            // Só loga usuário autenticado
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
                    Reason = "-",

                    Success = context.Response.StatusCode < 400,
                    StatusCode = context.Response.StatusCode,
                    ErrorMessage = errorMessage,

                    CreatedAt = DateTime.UtcNow,
                    Timestamp = DateTime.UtcNow
                };

                db.AuditLogs.Add(audit);
                await db.SaveChangesAsync();
            }
        }
    }
}