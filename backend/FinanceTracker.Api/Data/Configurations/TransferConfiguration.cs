using FinanceTracker.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Api.Data.Configurations;

public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
  public void Configure(EntityTypeBuilder<Transfer> builder)
  {
    builder.ToTable("transfers");
    builder.HasKey(t => t.Id);

    builder.Property(t => t.Id)
      .HasColumnName("id");

    builder.Property(t => t.UserId)
      .HasColumnName("user_id")
      .IsRequired();

    builder.Property(t => t.Type)
      .HasColumnName("type")
      .HasConversion<string>()
      .HasMaxLength(50)
      .IsRequired();

    builder.Property(t => t.FromAccountId)
      .HasColumnName("from_account_id")
      .IsRequired();

    builder.Property(t => t.ToAccountId)
      .HasColumnName("to_account_id")
      .IsRequired();

    builder.Property(t => t.Amount)
      .HasColumnName("amount")
      .HasPrecision(28, 18)
      .IsRequired();

    builder.Property(t => t.Currency)
      .HasColumnName("currency")
      .HasMaxLength(3)
      .IsRequired();

    builder.Property(t => t.Date)
      .HasColumnName("date")
      .IsRequired();

    builder.Property(t => t.Description)
      .HasColumnName("description")
      .HasMaxLength(200)
      .IsRequired();

    builder.Property(t => t.Notes)
      .HasColumnName("notes")
      .HasMaxLength(500);

    builder.Property(t => t.CreatedAt)
      .HasColumnName("created_at")
      .IsRequired();

    builder.Property(t => t.UpdatedAt)
      .HasColumnName("updated_at")
      .IsRequired();

    builder.HasOne<Account>()
      .WithMany()
      .HasForeignKey(t => t.FromAccountId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne<Account>()
      .WithMany()
      .HasForeignKey(t => t.ToAccountId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasIndex(t => t.UserId)
      .HasDatabaseName("ix_transfers_user_id");

    builder.HasIndex(t => new { t.UserId, t.Date })
      .HasDatabaseName("ix_transfers_user_id_date");

    builder.HasIndex(t => t.FromAccountId)
      .HasDatabaseName("ix_transfers_from_account_id");

    builder.HasIndex(t => t.ToAccountId)
      .HasDatabaseName("ix_transfers_to_account_id");
  }
}