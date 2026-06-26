using FinanceTracker.Api.Constants;
using FinanceTracker.Api.Data;
using FinanceTracker.Api.DTOs.Categories;
using FinanceTracker.Api.Models;
using FinanceTracker.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Services.Implementations;

public class CategoryService(FinanceTrackerDbContext context) : ICategoryService
{
  private readonly FinanceTrackerDbContext _context = context;

  public async Task<IReadOnlyList<Category>> GetRootsAsync()
  {
    // carico tutte le categorie dell'utente in un'unica query invece di fare
    // N + 1 query (una per radice + una per i figli di ciascuna)
    var allCategories = await _context.Categories
      .Where(c => c.UserId == AppConstants.HardcodedUserId || c.UserId == null) // include categorie di sistema
      .ToListAsync();

    var roots = allCategories.Where(c => c.IsRoot);

    foreach (var root in roots)
    {
      root.Children = [.. allCategories.Where(c => c.ParentCategoryId == root.Id)];
    }

    return roots.ToList().AsReadOnly();
  }

  public async Task<Category?> GetByIdAsync(Guid id) => await _context.Categories
    .FirstOrDefaultAsync(c => c.Id == id && (c.UserId == AppConstants.HardcodedUserId || c.UserId == null));

  public async Task<Category> AddAsync(CreateCategoryRequest request)
  {
    var isRoot = request.ParentCategoryId is null;

    if (request.ParentCategoryId is Guid parentCategoryId)
    {
      var parent = await GetByIdAsync(parentCategoryId)
        ?? throw new KeyNotFoundException($"Parent category {parentCategoryId} not found.");

      if (!parent.IsRoot)
      {
        throw new InvalidOperationException("Cannot create a subcategory of a subcategory. Maximum depth is 2 levels.");
      }
    }

    var category = new Category()
    {
      Id = Guid.NewGuid(),
      UserId = AppConstants.HardcodedUserId,
      Name = request.Name,
      Icon = request.Icon,
      ParentCategoryId = request.ParentCategoryId,
      IsRoot = isRoot
    };

    _context.Categories.Add(category);
    await _context.SaveChangesAsync();

    return category;
  }
  
  public async Task<Category?> UpdateAsync(Guid id, CreateCategoryRequest request)
  {
    var category = await GetByIdAsync(id);

    if (category is null)
    {
      return null;
    }

    category.Name = request.Name;
    category.Icon = request.Icon;

    await _context.SaveChangesAsync();

    return category;
  }

  public async Task<bool> DeleteAsync(Guid id)
  {
    var category = await GetByIdAsync(id);

    if (category is null)
    {
      return false;
    }

    bool hasChildren = await _context.Categories.AnyAsync(c => c.ParentCategoryId == id);
    
    if (hasChildren)
    {
      throw new InvalidOperationException("Cannot delete a root category that has subcategories. Delete subcategories first.");
    }

    _context.Categories.Remove(category);
    await _context.SaveChangesAsync();

    return true;
  }
}