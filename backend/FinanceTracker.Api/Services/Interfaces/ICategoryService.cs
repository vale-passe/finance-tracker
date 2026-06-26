using FinanceTracker.Api.DTOs.Categories;
using FinanceTracker.Api.Models;

namespace FinanceTracker.Api.Services.Interfaces;

public interface ICategoryService
{
  Task<IReadOnlyList<Category>> GetRootsAsync();
  Task<Category?> GetByIdAsync(Guid id);
  Task<Category> AddAsync(CreateCategoryRequest request);
  Task<Category?> UpdateAsync(Guid id, CreateCategoryRequest request);
  Task<bool> DeleteAsync(Guid id);
}