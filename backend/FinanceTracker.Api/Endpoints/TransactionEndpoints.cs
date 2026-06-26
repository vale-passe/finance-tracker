using FinanceTracker.Api.DTOs.Transactions;
using FinanceTracker.Api.Endpoints.Extensions;
using FinanceTracker.Api.Models;
using FinanceTracker.Api.Services.Interfaces;
using FinanceTracker.Api.Validators;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinanceTracker.Api.Endpoints;

public static class TransactionEndpoints
{
  public static void MapTransactionEndpoints(this WebApplication app)
  {
    var group = app.MapGroup("/api/transactions").WithTags("Transactions");

    group.MapGet("/", GetAll);
    group.MapGet("/{id:guid}", GetById).WithName("GetTransactionById");
    group.MapGet("/summary", GetSummary);

    group.MapPost("/", Create)
      .AddEndpointFilter<ValidationFilter<CreateTransactionRequest>>();

    group.MapPut("/{id:guid}", Update)
      .AddEndpointFilter<ValidationFilter<CreateTransactionRequest>>();

    group.MapDelete("/{id:guid}", Delete);
  }

  private static async Task<Ok<IEnumerable<TransactionResponse>>> GetAll(
    [AsParameters] TransactionFilters filters,
    ITransactionService service
  ) => TypedResults.Ok((await service.GetAllAsync(filters)).Select(t => t.ToResponse()));

  private static async Task<Results<Ok<TransactionResponse>, NotFound>> GetById(
    Guid id,
    ITransactionService service
  ) =>
    await service.GetByIdAsync(id) is Transaction transaction
      ? TypedResults.Ok(transaction.ToResponse())
      : TypedResults.NotFound();

  private static async Task<Ok<TransactionSummaryResponse>> GetSummary(ITransactionService service) =>
    TypedResults.Ok(await service.GetSummaryAsync());

  private static async Task<CreatedAtRoute<TransactionResponse>> Create(
    CreateTransactionRequest request,
    ITransactionService service
  )
  {
    var transaction = await service.AddAsync(request);
    return TypedResults.CreatedAtRoute(transaction.ToResponse(), "GetTransactionById", new { id = transaction.Id });
  }

  private static async Task<Results<Ok<TransactionResponse>, NotFound>> Update(
    Guid id,
    CreateTransactionRequest request,
    ITransactionService service
  ) =>
    await service.UpdateAsync(id, request) is Transaction transaction
      ? TypedResults.Ok(transaction.ToResponse())
      : TypedResults.NotFound();

  private static async Task<Results<NoContent, NotFound>> Delete(Guid id, ITransactionService service) =>
    await service.DeleteAsync(id)
      ? TypedResults.NoContent()
      : TypedResults.NotFound();
}