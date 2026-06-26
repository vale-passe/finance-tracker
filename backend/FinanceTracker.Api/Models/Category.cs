namespace FinanceTracker.Api.Models;

public class Category : BaseEntity
{
  public new Guid? UserId { get; set; } // override - null = categoria di sistema
  public string Name { get; set; } = string.Empty;
  public string? Icon { get; set; }
  public Guid? ParentCategoryId { get; set; } // null = categoria radice
  public bool IsRoot { get; set; } // true = non assegnabile a transazioni, solo per report
  public List<Category> Children { get; set; } = [];
}