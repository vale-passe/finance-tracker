using FinanceTracker.Api.DTOs.Liabilities;
using FinanceTracker.Api.Models;

namespace FinanceTracker.Api.Services.Interfaces;

public interface ILiabilityService
{
  Task<IReadOnlyList<Liability>> GetAllAsync();
  Task<Liability?> GetByIdAsync(Guid id);
  Task<Liability> AddAsync(CreateLiabilityRequest request);
  Task<Liability?> UpdateAsync(Guid id, UpdateLiabilityRequest request);
  Task<bool> DeleteAsync(Guid id);

  // aggiorna il capitale residuo dopo il pagamento di una rata
  Task UpdateRemainingAmountAsync(Guid liabilityId, decimal payedAmount);
}