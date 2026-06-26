using FinanceTracker.Api.DTOs.Accounts;
using FinanceTracker.Api.DTOs.Liabilities;
using FinanceTracker.Api.Endpoints.Extensions;
using FinanceTracker.Api.Models;
using FinanceTracker.Api.Services.Interfaces;
using FinanceTracker.Api.Validators;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinanceTracker.Api.Endpoints;

public static class LiabilityEndpoints
{
  public static void MapLiabilityEndpoints(this WebApplication app)
  {
    var group = app.MapGroup("/api/liabilitis").WithTags("Liabilities");

    group.MapGet("/", GetAll);
    group.MapGet("/{id:guid}", GetById).WithName("GetLiabilityById");

    // AddEnpointFilter<ValidationFilter<T>>() applica la validazione automaticamente
    // prima che la richiesta raggiunga l'handler - se il body non è valido,
    // risponse 400 senza chiamare il service
    group.MapPost("/", Create)
      .AddEndpointFilter<ValidationFilter<CreateLiabilityRequest>>();

    group.MapPut("/{id:guid}", Update)
      .AddEndpointFilter<ValidationFilter<UpdateLiabilityRequest>>();

    group.MapDelete("/{id:guid}", Delete);
  }

  private static async Task<Ok<IEnumerable<LiabiltyResponse>>> GetAll(ILiabilityService service) =>
    TypedResults.Ok((await service.GetAllAsync()).Select(a => a.ToResponse()));

  private static async Task<Results<Ok<LiabiltyResponse>, NotFound>> GetById(Guid id, ILiabilityService service) =>
    await service.GetByIdAsync(id) is Liability account
      ? TypedResults.Ok(account.ToResponse())
      : TypedResults.NotFound();

  private static async Task<CreatedAtRoute<LiabiltyResponse>> Create(
    CreateLiabilityRequest request,
    ILiabilityService service)
  {
    var liability = await service.AddAsync(request);
    return TypedResults.CreatedAtRoute(liability.ToResponse(), "GetLiabilityById", new { id = liability.Id });
  }

  private static async Task<Results<Ok<LiabiltyResponse>, NotFound>> Update(
    Guid id,
    UpdateLiabilityRequest request,
    ILiabilityService service
  ) =>
    await service.UpdateAsync(id, request) is Liability liability
      ? TypedResults.Ok(liability.ToResponse())
      : TypedResults.NotFound();

  private static async Task<Results<NoContent, NotFound>> Delete(Guid id, ILiabilityService service) =>
    await service.DeleteAsync(id)
      ? TypedResults.NoContent()
      : TypedResults.NotFound();
}