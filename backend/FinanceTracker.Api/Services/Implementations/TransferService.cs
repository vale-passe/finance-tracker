using System.Runtime.CompilerServices;
using FinanceTracker.Api.Constants;
using FinanceTracker.Api.Data;
using FinanceTracker.Api.DTOs.Transfers;
using FinanceTracker.Api.Models;
using FinanceTracker.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Services.Implementations;

public class TransferService(
  FinanceTrackerDbContext context,
  IAccountService accountService) : ITransferService
{
  private readonly FinanceTrackerDbContext _context = context;
  private readonly IAccountService _accountService = accountService;

  public async Task<IReadOnlyList<Transfer>> GetAllAsync() => await _context.Transfers
    .Where(t => t.UserId == AppConstants.HardcodedUserId)
    .OrderByDescending(t => t.Date)
    .ToListAsync();

  public async Task<Transfer?> GetByIdAsync(Guid id) => await _context.Transfers
    .FirstOrDefaultAsync(t => t.UserId == AppConstants.HardcodedUserId && t.Id == id);

  public async Task<Transfer> AddAsync(CreateTransferRequest request)
  {
    _ = await _accountService.GetByIdAsync(request.FromAccountId)
      ?? throw new KeyNotFoundException($"Source account {request.FromAccountId} not found.");

    _ = await _accountService.GetByIdAsync(request.ToAccountId)
      ?? throw new KeyNotFoundException($"Destination account {request.ToAccountId} not found.");

    var transfer = new Transfer()
    {
      Id = Guid.NewGuid(),
      UserId = AppConstants.HardcodedUserId,
      Type = request.Type,
      FromAccountId = request.FromAccountId,
      ToAccountId = request.ToAccountId,
      Amount = request.Amount,
      Currency = request.Currency,
      Date = request.Date,
      Description = request.Description,
      Notes = request.Notes
    };

    await using var dbTransaction = await _context.Database.BeginTransactionAsync();

    try
    {
      _context.Transfers.Add(transfer);
      await _context.SaveChangesAsync();

      await _accountService.UpdateBalanceAsync(request.FromAccountId, -request.Amount);
      await _accountService.UpdateBalanceAsync(request.ToAccountId, request.Amount);

      await dbTransaction.CommitAsync();
    }
    catch
    {
      await dbTransaction.RollbackAsync();
      throw;
    }

    return transfer;
  }

  public async Task<bool> DeleteAsync(Guid id)
  {
    var transfer = await GetByIdAsync(id);

    if (transfer is null)
    {
      return false;
    }

    await using var dbTransaction = await _context.Database.BeginTransactionAsync();

    try
    {
      await _accountService.UpdateBalanceAsync(transfer.FromAccountId, transfer.Amount);
      await _accountService.UpdateBalanceAsync(transfer.ToAccountId, -transfer.Amount);

      _context.Transfers.Remove(transfer);
      await _context.SaveChangesAsync();

      await dbTransaction.CommitAsync();
    }
    catch
    {
      await dbTransaction.RollbackAsync();
      throw;
    }

    return true;
  }
}