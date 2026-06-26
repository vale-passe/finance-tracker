namespace FinanceTracker.Api.DTOs.Categories;

public record CategoryResponse(
  Guid Id,
  string Name,
  string? Icon,
  Guid? ParentCategoryId,
  bool IsRoot,
  DateTimeOffset CreatedAt,
  DateTimeOffset UpdatedAt
)
{
  public List<CategoryResponse> Children { get; set; } = [];
};