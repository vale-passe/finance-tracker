using FinanceTracker.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Api.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
  public void Configure(EntityTypeBuilder<Account> builder)
  {
    builder.ToTable("accounts");
    builder.HasKey(a => a.Id);

    builder.Property(a => a.Id)
      .HasColumnName("id");

    builder.Property(a => a.UserId)
      .HasColumnName("user_id")
      .IsRequired();

    builder.Property(a => a.Name)
      .HasColumnName("name")
      .HasMaxLength(100)
      .IsRequired();

    builder.Property(a => a.Type)
      .HasColumnName("type")
      .HasConversion<string>()
      .HasMaxLength(50)
      .IsRequired();

    // nullable per Investment e Crypto
    builder.Property(a => a.Pillar)
      .HasColumnName("pillar")
      .HasConversion<string>()
      .HasMaxLength(50);

    builder.Property(a => a.Currency)
      .HasColumnName("currency")
      .HasMaxLength(3)
      .IsRequired();

    builder.Property(a => a.Balance)
      .HasColumnName("balance")
      .HasPrecision(28, 18)
      .IsRequired();

    builder.Property(a => a.Color)
      .HasColumnName("color")
      .HasMaxLength(7)
      .IsRequired();

    builder.Property(a => a.IsAchived)
      .HasColumnName("is_archived")
      .HasDefaultValue(false)
      .IsRequired();

    builder.Property(a => a.Notes)
      .HasColumnName("notes")
      .HasMaxLength(500);

    builder.Property(a => a.CreatedAt)
      .HasColumnName("created_at")
      .IsRequired();

    builder.Property(a => a.UpdatedAt)
      .HasColumnName("updated_at")
      .IsRequired();

    // indice su UserId - ogni query filtra su utente
    builder.HasIndex(a => a.UserId)
      .HasDatabaseName("ix_accounts_user_id");

    // indice composto per query frequenti: utente + tipo di conto
    builder.HasIndex(a => new { a.UserId, a.Type })
      .HasDatabaseName("ix_accounts_user_id_type");
  }
}