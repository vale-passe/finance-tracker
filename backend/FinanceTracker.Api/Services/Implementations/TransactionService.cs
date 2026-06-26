using System.Drawing;
using System.Runtime.CompilerServices;
using FinanceTracker.Api.Constants;
using FinanceTracker.Api.Data;
using FinanceTracker.Api.DTOs.Transactions;
using FinanceTracker.Api.Models;
using FinanceTracker.Api.Models.Enums;
using FinanceTracker.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Services.Implementations;

public class TransactionService(
  FinanceTrackerDbContext context,
  IAccountService accountService
) : ITransactionService
{
  private readonly FinanceTrackerDbContext _context = context;
  private readonly IAccountService _accountService = accountService;

  public async Task<IReadOnlyList<Transaction>> GetAllAsync(TransactionFilters filters)
  {
    var query = _context.Transactions
      .Where(t => t.UserId == AppConstants.HardcodedUserId)
      .AsQueryable();

    if (filters.From is not null)
    {
      query = query.Where(t => t.Date >= filters.From);
    }

    if (filters.To is not null)
    {
      query = query.Where(t => t.Date <= filters.To);
    }

    if (filters.AccountId is not null)
    {
      query = query.Where(t => t.AccountId == filters.AccountId);
    }

    if (filters.CategoryId is not null)
    {
      query = query.Where(t => t.CategoryId == filters.CategoryId);
    }

    if (filters.Type is not null)
    {
      query = query.Where(t => t.Type == filters.Type);
    }

    return await query.OrderByDescending(t => t.Date).ToListAsync();
  }

  public async Task<Transaction?> GetByIdAsync(Guid id) => await _context.Transactions
    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == AppConstants.HardcodedUserId);

  public async Task<TransactionSummaryResponse> GetSummaryAsync()
  {
    var result = await _context.Transactions
      .Where(t => t.UserId == AppConstants.HardcodedUserId)
      .GroupBy(_ => 1) // raggruppa tutto in un unico gruppo
      .Select(g => new
      {
        TotalIncome = g
          .Where(t => t.Type == TransactionType.Income)
          .Sum(t => (decimal?)t.Amount) ?? 0m,
        TotalExpenses = g
          .Where(t => t.Type == TransactionType.Expense)
          .Sum(t => (decimal?)t.Amount) ?? 0m,
      })
      .FirstOrDefaultAsync();

    var totalIncome = result?.TotalIncome ?? 0m;
    var totalExpenses = result?.TotalExpenses ?? 0m;

    return new TransactionSummaryResponse(
      TotalIncome: totalIncome,
      TotalExpenses: totalExpenses,
      Balance: totalIncome - totalExpenses
    );
  }

  public async Task<Transaction> AddAsync(CreateTransactionRequest request)
  {
    _ = _accountService.GetByIdAsync(request.AccountId)
      ?? throw new KeyNotFoundException($"Account {request.AccountId} not found.");

    var transaction = new Transaction()
    {
      Id = Guid.NewGuid(),
      UserId = AppConstants.HardcodedUserId,
      Type = request.Type,
      Amount = request.Amount,
      Currency = request.Currency,
      Date = request.Date,
      Description = request.Description,
      AccountId = request.AccountId,
      CategoryId = request.CategoryId,
      LiabilityId = request.LiabilityId,
      IsCategorized = request.CategoryId is not null,
      Notes = request.Notes
    };

    // operazione atomica: salva la transazione E aggiorna il balance
    // in un'unica transazione database - o tutto va a buon fine o niente

    await using var dbTransaction = await _context.Database.BeginTransactionAsync();

    try
    {
      _context.Transactions.Add(transaction);
      await _context.SaveChangesAsync();

      var delta = request.Type == TransactionType.Income ? request.Amount : -request.Amount;
      await _accountService.UpdateBalanceAsync(request.AccountId, delta);

      await dbTransaction.CommitAsync();
    }
    catch
    {
      await dbTransaction.RollbackAsync();
      throw;
    }

    return transaction;
  }

  public async Task<Transaction?> UpdateAsync(Guid id, CreateTransactionRequest request)
  {
    var transaction = await GetByIdAsync(id);

    if (transaction is null)
    {
      return null;
    }

    // verifico che il nuovo AccountId esista PRIMA di modificare qualsiasi balance
    _ = await _accountService.GetByIdAsync(request.AccountId)
      ?? throw new KeyNotFoundException($"Account {request.AccountId} not found.");

    await using var dbTransaction = await _context.Database.BeginTransactionAsync();

    try
    {
      // 1. annullo l'effetto della transazione originale sul balance
      var reverseDelta = transaction.Type == TransactionType.Expense ? transaction.Amount : -transaction.Amount;
      await _accountService.UpdateBalanceAsync(transaction.AccountId, reverseDelta);

      // 2. aggiorno i campi
      transaction.Type = request.Type;
      transaction.Amount = request.Amount;
      transaction.Currency = request.Currency;
      transaction.Date = request.Date;
      transaction.Description = request.Description;
      transaction.AccountId = request.AccountId;
      transaction.CategoryId = request.CategoryId;
      transaction.LiabilityId = request.LiabilityId;
      transaction.IsCategorized = request.CategoryId is not null;
      transaction.Notes = request.Notes;

      await _context.SaveChangesAsync();

      // 3. applico l'effetto della transazione aggiornata sul balance 
      var newDelta = request.Type == TransactionType.Income ? request.Amount : -request.Amount;
      await _accountService.UpdateBalanceAsync(request.AccountId, newDelta);

      await dbTransaction.CommitAsync();
    }
    catch
    {
      await dbTransaction.RollbackAsync();
      throw;
    }

    return transaction;
  }

  public async Task<bool> DeleteAsync(Guid id)
  {
    var transaction = await GetByIdAsync(id);

    if (transaction is null)
    {
      return false;
    }

    await using var dbTransaction = await _context.Database.BeginTransactionAsync();

    try
    {
      var reverseDelta = transaction.Type == TransactionType.Expense ? transaction.Amount : -transaction.Amount;
      await _accountService.UpdateBalanceAsync(transaction.AccountId, reverseDelta);

      _context.Transactions.Remove(transaction);
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