namespace FinanceTracker.Api.DTOs.Categories;

public record CreateCategoryRequest(
  string Name,
  string? Icon,
  Guid? ParentCategoryId
);