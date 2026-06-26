using FinanceTracker.Api.Constants;
using FinanceTracker.Api.Data;
using FinanceTracker.Api.DTOs.Accounts;
using FinanceTracker.Api.Models;
using FinanceTracker.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Services.Implementations;

public class AccountService(FinanceTrackerDbContext context) : IAccountService
{
  private readonly FinanceTrackerDbContext _context = context;

  public async Task<IReadOnlyList<Account>> GetAllAsync() => await _context.Accounts
    .Where(a => a.UserId == AppConstants.HardcodedUserId)
    .OrderBy(a => a.Name)
    .ToListAsync();

  public async Task<Account?> GetByIdAsync(Guid id) => await _context.Accounts
    .FirstOrDefaultAsync(a => a.Id == id && a.UserId == AppConstants.HardcodedUserId);

  public async Task<Account> AddAsync(CreateAccountRequest request)
  {
    var account = new Account
    {
      Id = Guid.NewGuid(),
      UserId = AppConstants.HardcodedUserId,
      Name = request.Name,
      Type = request.Type,
      Pillar = request.Pillar,
      Currency = request.Currency,
      Balance = 0m,
      Color = request.Color,
      Notes = request.Notes
    };

    _context.Accounts.Add(account);
    await _context.SaveChangesAsync();

    return account;
  }

  public async Task<Account?> UpdateAsync(Guid id, CreateAccountRequest request)
  {
    var account = await GetByIdAsync(id);

    if (account is null)
    {
      return null;
    }

    account.Name = request.Name;
    account.Type = request.Type;
    account.Pillar = request.Pillar;
    account.Currency = request.Currency;
    account.Color = request.Color;
    account.Notes = request.Notes;

    await _context.SaveChangesAsync();

    return account;
  }

  public async Task<bool> DeleteAsync(Guid id)
  {
    var account = await GetByIdAsync(id);

    if (account is null)
    {
      return false;
    }

    var hasTransactions = await _context.Transactions
      .AnyAsync(t => t.AccountId == id);

    if (hasTransactions)
    {
      throw new InvalidOperationException(
        $"Cannot delete account '{account.Name}' because it has " +
        "linked transactions. Delete or reassign them first."
      );
    }

    var hasTransfers = await _context.Transfers
      .AnyAsync(t => t.FromAccountId == id || t.ToAccountId == id);

    if (hasTransfers)
    {
      throw new InvalidOperationException(
        $"Cannot delete account '{account.Name}' because it has " +
        "linked transfers. Delete or reassign them first."
      );
    }

    var hasLiabilities = await _context.Liabilities
      .AnyAsync(l => l.AccountId == id);

    if (hasLiabilities)
    {
      throw new InvalidOperationException(
        $"Cannot delete account '{account.Name}' because it has " +
        "linked liabilities. Close them first."
      );
    }

    _context.Accounts.Remove(account);
    await _context.SaveChangesAsync();

    return true;
  }

  // ExecuteUpdateAsync aggiorna direttamente sul db senza caricare l'entità
  // in memoria - più efficiente di Load + Modify + Save per operazioni su
  // singola colonna
  public async Task UpdateBalanceAsync(Guid accountId, decimal delta) =>
    await _context.Accounts
      .Where(a => a.Id == accountId)
      .ExecuteUpdateAsync(_ => _
        .SetProperty(a => a.Balance, a => a.Balance + delta)
        .SetProperty(a => a.UpdatedAt, DateTimeOffset.UtcNow));
}