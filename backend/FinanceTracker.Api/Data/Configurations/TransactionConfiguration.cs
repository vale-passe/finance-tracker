using FinanceTracker.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Api.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
  public void Configure(EntityTypeBuilder<Transaction> builder)
  {
    builder.ToTable("transactions");
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

    builder.Property(t => t.AccountId)
      .HasColumnName("account_id")
      .IsRequired();

    builder.Property(t => t.CategoryId)
      .HasColumnName("category_id");

    builder.Property(t => t.LiabilityId)
      .HasColumnName("liability_id");

    builder.Property(t => t.IsCategorized)
      .HasColumnName("is_categorized")
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

    // FK verso Account - Restrict: non posso eliminare un Account che ha
    // transazioni
    builder.HasOne<Account>()
      .WithMany()
      .HasForeignKey(t => t.AccountId)
      .OnDelete(DeleteBehavior.Restrict);

    // FK verso Category - SetNull: se elimino una categoria, le transazioni
    // collegate diventano non categorizzate invece di essere eliminate
    builder.HasOne<Category>()
      .WithMany()
      .HasForeignKey(t => t.CategoryId)
      .OnDelete(DeleteBehavior.SetNull);

    // FK verso Liability - SetNull: se elimino una liability, le rate
    // collegate diventano transazioni normali invece di essere eliminate
    builder.HasOne<Liability>()
      .WithMany()
      .HasForeignKey(t => t.LiabilityId)
      .OnDelete(DeleteBehavior.SetNull);

    builder.HasIndex(t => t.UserId)
      .HasDatabaseName("ix_transactions_user_id");

    builder.HasIndex(t => new { t.UserId, t.Date })
      .HasDatabaseName("ix_transactions_user_id_date");

    builder.HasIndex(t => new { t.UserId, t.AccountId })
      .HasDatabaseName("ix_transactions_user_id_account_id");

    builder.HasIndex(t => new { t.UserId, t.IsCategorized })
      .HasDatabaseName("ix_transactions_user_id_is_categorized");
  }
}