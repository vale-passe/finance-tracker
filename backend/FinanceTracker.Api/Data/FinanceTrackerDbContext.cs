using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using FinanceTracker.Api.Constants;
using FinanceTracker.Api.Models;
using FinanceTracker.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FinanceTracker.Api.Data;

public class FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options) : DbContext(options)
{
  public DbSet<Account> Accounts => Set<Account>();
  public DbSet<Category> Categories => Set<Category>();
  public DbSet<Transaction> Transactions => Set<Transaction>();
  public DbSet<Transfer> Transfers => Set<Transfer>();
  public DbSet<Liability> Liabilities => Set<Liability>();
  public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceTrackerDbContext).Assembly);
  }

  public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
  {
    var now = DateTimeOffset.UtcNow;
    var auditEntries = new List<AuditLog>();

    foreach (var entry in ChangeTracker.Entries<BaseEntity>())
    {
      // aggiorno il timestamp
      switch (entry.State)
      {
        case EntityState.Added:
          entry.Entity.CreatedAt = now;
          entry.Entity.UpdatedAt = now;
          break;

        case EntityState.Modified:
          entry.Entity.UpdatedAt = now;
          break;
      }

      // costruisco il record di audit
      var action = entry.State switch
      {
        EntityState.Added => AuditAction.Created,
        EntityState.Modified => AuditAction.Updated,
        EntityState.Deleted => AuditAction.Deleted,
        _ => (AuditAction?)null
      };

      if (action is not AuditAction auditAction)
      {
        continue;
      }

      var changes = BuildChanges(entry, auditAction);

      auditEntries.Add(new AuditLog()
      {
        Id = Guid.NewGuid(),
        UserId = AppConstants.HardcodedUserId,
        EntityType = entry.Entity.GetType().Name,
        EntityId = entry.Entity.Id,
        Action = auditAction,
        ChangedAt = now,
        Changes = changes,
        Source = AuditSource.Manual
      });
    }

    var result = await base.SaveChangesAsync(ct);

    // salvo i log di audit dopo il salvataggio principale - se il salvataggio
    // principale fallisce, i log non vengono creati
    if (auditEntries.Count > 0)
    {
      AuditLogs.AddRange(auditEntries);
      await base.SaveChangesAsync(ct);
    }

    return result;
  }

  private static string BuildChanges(EntityEntry<BaseEntity> entry, AuditAction action)
  {
    var changes = new Dictionary<string, object?>();

    if (action == AuditAction.Created)
    {
      // per le creazioni salvo tutti i valori correnti
      foreach (var property in entry.Properties)
      {
        changes[property.Metadata.Name] = property.CurrentValue;
      }
    }
    else if (action == AuditAction.Updated)
    {
      // per le modifiche salvo solo i campi cambiati con before/after
      foreach (var property in entry.Properties)
      {
        if (property.IsModified)
        {
          changes[property.Metadata.Name] = new
          {
            before = property.OriginalValue,
            after = property.CurrentValue
          };
        }
      }
    }
    else if (action == AuditAction.Deleted)
    {
      // per le eliminazioni salvo i valori originali
      foreach (var property in entry.Properties)
      {
        changes[property.Metadata.Name] = property.OriginalValue;
      }
    }

    return JsonSerializer.Serialize(changes);
  }
}