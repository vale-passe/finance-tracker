using FinanceTracker.Api.DTOs.Transfers;
using FinanceTracker.Api.Endpoints.Extensions;
using FinanceTracker.Api.Models;
using FinanceTracker.Api.Services.Interfaces;
using FinanceTracker.Api.Validators;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinanceTracker.Api.Endpoints;

public static class TransferEndpoints
{
  public static void MapTransferEndpoints(this WebApplication app)
  {
    var group = app.MapGroup("/api/transfers").WithTags("Transfers");

    group.MapGet("/", GetAll);
    group.MapGet("/{id:guid}", GetById).WithName("GetTransferById");
    
    group.MapPost("/", Create)
      .AddEndpointFilter<ValidationFilter<CreateTransferRequest>>();

    group.MapDelete("/{id:guid}", Delete);
  }

  private static async Task<Ok<IEnumerable<TransferResponse>>> GetAll(ITransferService service) =>
    TypedResults.Ok((await service.GetAllAsync()).Select(t => t.ToResponse()));

  private static async Task<Results<Ok<TransferResponse>, NotFound>> GetById(Guid id, ITransferService service) =>
    await service.GetByIdAsync(id) is Transfer transfer
      ? TypedResults.Ok(transfer.ToResponse())
      : TypedResults.NotFound();

  private static async Task<CreatedAtRoute<TransferResponse>> Create(
    CreateTransferRequest request,
    ITransferService service)
  {
    var transfer = await service.AddAsync(request);
    return TypedResults.CreatedAtRoute(transfer.ToResponse(), "GetTransferById", new { id = transfer.Id });
  }

  private static async Task<Results<NoContent, NotFound>> Delete(Guid id, ITransferService service) =>
    await service.DeleteAsync(id)
      ? TypedResults.NoContent()
      : TypedResults.NotFound();
}