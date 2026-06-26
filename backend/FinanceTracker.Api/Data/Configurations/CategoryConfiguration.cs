using FinanceTracker.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Api.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
  public void Configure(EntityTypeBuilder<Category> builder)
  {
    builder.ToTable("categories");
    builder.HasKey(c => c.Id);

    builder.Property(c => c.Id)
      .HasColumnName("id");

    // nullable per le categorie di sistema
    builder.Property(c => c.UserId)
      .HasColumnName("user_id");

    builder.Property(c => c.Name)
      .HasColumnName("name")
      .HasMaxLength(100)
      .IsRequired();

    builder.Property(c => c.Icon)
      .HasColumnName("icon")
      .HasMaxLength(50);

    builder.Property(c => c.ParentCategoryId)
      .HasColumnName("parent_category_id");

    builder.Property(c => c.IsRoot)
      .HasColumnName("is_root")
      .IsRequired();

    builder.Property(c => c.CreatedAt)
      .HasColumnName("created_at")
      .IsRequired();

    builder.Property(c => c.UpdatedAt)
      .HasColumnName("updated_at")
      .IsRequired();

    // relazione self-referencing
    // restrict: il db rifiuta l'eliminazione di categorie con figli - è sull'HasOne
    builder.HasOne<Category>()
      .WithMany()
      .HasForeignKey(c => c.ParentCategoryId)
      .OnDelete(DeleteBehavior.Restrict);

    // children non è una colonna - viene popolata dal service
    builder.Ignore(c => c.Children);

    builder.HasIndex(c => c.UserId)
      .HasDatabaseName("ix_categories_user_id");

    builder.HasIndex(c => c.ParentCategoryId)
      .HasDatabaseName("ix_categories_parent_category_id");
  }
}