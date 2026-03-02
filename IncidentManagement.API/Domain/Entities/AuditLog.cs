namespace IncidentManagement.API.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Action { get; set; } = null!;

    public string PerformedBy { get; set; } = null!;

    public string TargetUser { get; set; } = null!;

    public string OldRole { get; set; } = null!;

    public string NewRole { get; set; } = null!;

    public string Reason { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime Timestamp { get; set; }
}