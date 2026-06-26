using FinanceTracker.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Api.Data.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
  public void Configure(EntityTypeBuilder<AuditLog> builder)
  {
    builder.ToTable("audit_logs");
    builder.HasKey(a => a.Id);

    builder.Property(a => a.Id)
      .HasColumnName("id");

    builder.Property(a => a.UserId)
      .HasColumnName("user_id")
      .IsRequired();

    builder.Property(a => a.EntityType)
      .HasColumnName("entity_type")
      .HasMaxLength(100)
      .IsRequired();

    builder.Property(a => a.EntityId)
      .HasColumnName("entity_id")
      .IsRequired();

    builder.Property(a => a.Action)
      .HasColumnName("action")
      .HasConversion<string>()
      .HasMaxLength(50)
      .IsRequired();

    builder.Property(a => a.ChangedAt)
     .HasColumnName("changed_at")
     .IsRequired();

    // jsonb - tipo PostgreSQL che indicizza il contenuto JSON
    // permette query sul contenuto: WHERE changes -> 'Amount' IS NOT NULL
    builder.Property(a => a.Changes)
      .HasColumnName("changes")
      .HasColumnType("jsonb")
      .IsRequired();

    builder.Property(a => a.Source)
     .HasColumnName("source")
     .HasConversion<string>()
     .HasMaxLength(50)
     .IsRequired();

    builder.HasIndex(a => a.UserId)
      .HasDatabaseName("ix_audit_logs_user_id");

    builder.HasIndex(a => new { a.EntityType, a.EntityId })
      .HasDatabaseName("ix_audit_logs_entity");

    builder.HasIndex(a => a.ChangedAt)
      .HasDatabaseName("ix_audit_logs_changed_at");
  }
}