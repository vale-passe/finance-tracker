using FinanceTracker.Api.DTOs.Accounts;
using FinanceTracker.Api.Endpoints.Extensions;
using FinanceTracker.Api.Models;
using FinanceTracker.Api.Services.Interfaces;
using FinanceTracker.Api.Validators;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinanceTracker.Api.Endpoints;

public static class AccountEndpoints
{
  public static void MapAccountEndpoints(this WebApplication app)
  {
    var group = app.MapGroup("/api/accounts").WithTags("Accounts");

    group.MapGet("/", GetAll);
    group.MapGet("/{id:guid}", GetById).WithName("GetAccountById");

    // AddEnpointFilter<ValidationFilter<T>>() applica la validazione automaticamente
    // prima che la richiesta raggiunga l'handler - se il body non è valido,
    // risponse 400 senza chiamare il service
    group.MapPost("/", Create)
      .AddEndpointFilter<ValidationFilter<CreateAccountRequest>>();

    group.MapPut("/{id:guid}", Update)
      .AddEndpointFilter<ValidationFilter<CreateAccountRequest>>();

    group.MapDelete("/{id:guid}", Delete);
  }

  private static async Task<Ok<IEnumerable<AccountResponse>>> GetAll(IAccountService service) =>
    TypedResults.Ok((await service.GetAllAsync()).Select(a => a.ToResponse()));

  private static async Task<Results<Ok<AccountResponse>, NotFound>> GetById(Guid id, IAccountService service) =>
    await service.GetByIdAsync(id) is Account account
      ? TypedResults.Ok(account.ToResponse())
      : TypedResults.NotFound();

  private static async Task<CreatedAtRoute<AccountResponse>> Create(
    CreateAccountRequest request,
    IAccountService service)
  {
    var account = await service.AddAsync(request);
    return TypedResults.CreatedAtRoute(account.ToResponse(), "GetAccountById", new { id = account.Id });
  }

  private static async Task<Results<Ok<AccountResponse>, NotFound>> Update(
    Guid id,
    CreateAccountRequest request,
    IAccountService service
  ) =>
    await service.UpdateAsync(id, request) is Account account
      ? TypedResults.Ok(account.ToResponse())
      : TypedResults.NotFound();

  private static async Task<Results<NoContent, NotFound>> Delete(Guid id, IAccountService service) =>
    await service.DeleteAsync(id)
      ? TypedResults.NoContent()
      : TypedResults.NotFound();
}