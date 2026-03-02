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

        // Executa request primeiro
        await _next(context);

        // Só registra se usuário autenticado
        if (username != null && username != "Anonymous")
        {
            var audit = new AuditLog
            {
                Id = Guid.NewGuid(),
                Action = $"{method} {path}",
                PerformedBy = username,
                TargetUser = "-",
                OldRole = "-",
                NewRole = "-",
                CreatedAt = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow
            };

            db.AuditLogs.Add(audit);
            await db.SaveChangesAsync();
        }
    }
}