using FinanceTracker.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Api.Data.Configurations;

public class LiabilityConfiguration : IEntityTypeConfiguration<Liability>
{
  public void Configure(EntityTypeBuilder<Liability> builder)
  {
    builder.ToTable("liabilities");
    builder.HasKey(l => l.Id);

    builder.Property(l => l.Id)
      .HasColumnName("id");

    builder.Property(l => l.UserId)
      .HasColumnName("user_id")
      .IsRequired();

    builder.Property(l => l.Name)
      .HasColumnName("name")
      .HasMaxLength(100)
      .IsRequired();

    builder.Property(t => t.Type)
      .HasColumnName("type")
      .HasConversion<string>()
      .HasMaxLength(50)
      .IsRequired();

    builder.Property(l => l.AccountId)
      .HasColumnName("account_id")
      .IsRequired();

    builder.Property(l => l.OriginalAmount)
      .HasColumnName("original_amount")
      .HasPrecision(28, 18)
      .IsRequired();

    builder.Property(l => l.RemainingAmount)
      .HasColumnName("remaining_amount")
      .HasPrecision(28, 18)
      .IsRequired();

    builder.Property(l => l.InterestRate)
      .HasColumnName("interest_rate")
      .HasPrecision(8, 6)
      .IsRequired();

    builder.Property(l => l.InstallmentAmount)
      .HasColumnName("installment_amount")
      .HasPrecision(28, 18)
      .IsRequired();

    builder.Property(l => l.InstallmentFrequency)
      .HasColumnName("installment_frequency")
      .HasConversion<string>()
      .HasMaxLength(50)
      .IsRequired();

    builder.Property(l => l.StartDate)
      .HasColumnName("start_date")
      .IsRequired();

    builder.Property(l => l.EndDate)
      .HasColumnName("end_date")
      .IsRequired();

    builder.Property(l => l.Pillar)
      .HasColumnName("pillar")
      .HasConversion<string>()
      .HasMaxLength(50)
      .IsRequired();

    builder.Property(l => l.Notes)
      .HasColumnName("notes")
      .HasMaxLength(500);

    builder.Property(l => l.CreatedAt)
      .HasColumnName("created_at")
      .IsRequired();

    builder.Property(l => l.UpdatedAt)
      .HasColumnName("updated_at")
      .IsRequired();

    // FK verso Account - Restrict: non posso eliminare un Account che ha
    // una Liability attiva
    builder.HasOne<Account>()
      .WithMany()
      .HasForeignKey(l => l.AccountId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasIndex(l => l.UserId)
      .HasDatabaseName("ix_liabilities_user_id");

    builder.HasIndex(l => new { l.UserId, l.Type })
      .HasDatabaseName("ix_transactions_user_id_type");
  }
}