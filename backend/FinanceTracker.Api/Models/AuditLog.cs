using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.Models;

public class AuditLog
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public string EntityType { get; set; } = string.Empty;
  public Guid EntityId { get; set; }
  public AuditAction Action { get; set; }
  public DateTimeOffset ChangedAt { get; set; }
  public string Changes { get; set; } = string.Empty; // jsonb
  public AuditSource Source { get; set; }
}