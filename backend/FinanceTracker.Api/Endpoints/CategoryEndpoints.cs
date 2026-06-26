using System.Runtime.CompilerServices;
using FinanceTracker.Api.DTOs.Categories;
using FinanceTracker.Api.Endpoints.Extensions;
using FinanceTracker.Api.Models;
using FinanceTracker.Api.Services.Interfaces;
using FinanceTracker.Api.Validators;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinanceTracker.Api.Endpoints;

public static class CategoryEndpoints
{
  public static void MapCategoryEndpoints(this WebApplication app)
  {
    var group = app.MapGroup("/api/categories").WithTags("Categories");

    group.MapGet("/", GetRoots);
    group.MapGet("/{id:guid}", GetById).WithName("GetCategoryById");

    group.MapPost("/", Create)
      .AddEndpointFilter<ValidationFilter<CreateCategoryRequest>>();

    group.MapPut("/{id:guid}", Update)
      .AddEndpointFilter<ValidationFilter<CreateCategoryRequest>>();

    group.MapDelete("/{id:guid}", Delete);
  }

  private static async Task<Ok<IEnumerable<CategoryResponse>>> GetRoots(ICategoryService service) =>
    TypedResults.Ok((await service.GetRootsAsync()).Select(r => r.ToResponse()));

  private static async Task<Results<Ok<CategoryResponse>, NotFound>> GetById(
    Guid id,
    ICategoryService service
  ) =>
    await service.GetByIdAsync(id) is Category category
      ? TypedResults.Ok(category.ToResponse())
      : TypedResults.NotFound();

  private static async Task<CreatedAtRoute<CategoryResponse>> Create(
    CreateCategoryRequest request,
    ICategoryService service
  )
  {
    var category = await service.AddAsync(request);
    return TypedResults.CreatedAtRoute(category.ToResponse(), "GetCategoryById", new { id = category.Id });
  }

  private static async Task<Results<Ok<CategoryResponse>, NotFound>> Update(
    Guid id,
    CreateCategoryRequest request,
    ICategoryService service
    ) =>
      await service.UpdateAsync(id, request) is Category category
        ? TypedResults.Ok(category.ToResponse())
        : TypedResults.NotFound();

  private static async Task<Results<NoContent, NotFound>> Delete(Guid id, ICategoryService service) =>
    await service.DeleteAsync(id)
      ? TypedResults.NoContent()
      : TypedResults.NotFound();
}