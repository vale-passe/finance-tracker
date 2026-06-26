using FinanceTracker.Api.DTOs.Accounts;
using FinanceTracker.Api.Models;

namespace FinanceTracker.Api.Services.Interfaces;

public interface IAccountService
{
  Task<IReadOnlyList<Account>> GetAllAsync();
  Task<Account?> GetByIdAsync(Guid id);
  Task<Account> AddAsync(CreateAccountRequest request);
  Task<Account?> UpdateAsync(Guid id, CreateAccountRequest request);
  Task<bool> DeleteAsync(Guid id);

  // chiamato internamente da TransactionService e TransferService
  // delta positivo = aumento, delta negativo = diminuzione
  Task UpdateBalanceAsync(Guid accountId, decimal delta);
}