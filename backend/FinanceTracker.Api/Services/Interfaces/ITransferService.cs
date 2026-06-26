using FinanceTracker.Api.DTOs.Transfers;
using FinanceTracker.Api.Models;

namespace FinanceTracker.Api.Services.Interfaces;

public interface ITransferService
{
  Task<IReadOnlyList<Transfer>> GetAllAsync();
  Task<Transfer?> GetByIdAsync(Guid id);
  Task<Transfer> AddAsync(CreateTransferRequest request);
  Task<bool> DeleteAsync(Guid id);
}