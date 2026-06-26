using FinanceTracker.Api.DTOs.Transactions;
using FinanceTracker.Api.Models;

namespace FinanceTracker.Api.Services.Interfaces;

public interface ITransactionService
{
  Task<IReadOnlyList<Transaction>> GetAllAsync(TransactionFilters filters);
  Task<Transaction?> GetByIdAsync(Guid id);
  Task<Transaction> AddAsync(CreateTransactionRequest request);
  Task<Transaction?> UpdateAsync(Guid id, CreateTransactionRequest request);
  Task<bool> DeleteAsync(Guid id);
  Task<TransactionSummaryResponse> GetSummaryAsync();
}