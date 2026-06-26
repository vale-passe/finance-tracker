using FinanceTracker.Api.Constants;
using FinanceTracker.Api.Data;
using FinanceTracker.Api.DTOs.Liabilities;
using FinanceTracker.Api.Models;
using FinanceTracker.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Services.Implementations;

public class LiabilityService(
  FinanceTrackerDbContext context,
  IAccountService accountService
) : ILiabilityService
{
  private readonly FinanceTrackerDbContext _context = context;
  private readonly IAccountService _accountService = accountService;

  public async Task<IReadOnlyList<Liability>> GetAllAsync() => await _context.Liabilities
    .Where(l => l.UserId == AppConstants.HardcodedUserId)
    .OrderBy(l => l.Name)
    .ToListAsync();

  public async Task<Liability?> GetByIdAsync(Guid id) => await _context.Liabilities
    .FirstOrDefaultAsync(l => l.Id == id && l.UserId == AppConstants.HardcodedUserId);

  public async Task<Liability> AddAsync(CreateLiabilityRequest request)
  {
    var _ = await _accountService.GetByIdAsync(request.AccountId)
      ?? throw new KeyNotFoundException($"Account {request.AccountId} not found");

    var liability = new Liability
    {
      Id = Guid.NewGuid(),
      UserId = AppConstants.HardcodedUserId,
      Name = request.Name,
      Type = request.Type,
      AccountId = request.AccountId,
      OriginalAmount = request.OriginalAmount,
      RemainingAmount = request.OriginalAmount, // parte dal debito pieno
      InterestRate = request.InterestRate,
      InstallmentAmount = request.InstallmentAmount,
      InstallmentFrequency = request.InstallmentFrequency,
      StartDate = request.StartDate,
      EndDate = request.EndDate,
      Pillar = request.Pillar,
      Notes = request.Notes
    };

    _context.Liabilities.Add(liability);
    await _context.SaveChangesAsync();

    return liability;
  }

  public async Task<Liability?> UpdateAsync(Guid id, UpdateLiabilityRequest request)
  {
    var liability = await GetByIdAsync(id);

    if (liability is null)
    {
      return null;
    }

    // OriginalAmount e StartDate non si modificano dopo la creazione - sono
    // dati storici del contratto
    liability.Name = request.Name;
    liability.InterestRate = request.InterestRate;
    liability.InstallmentAmount = request.InstallmentAmount;
    liability.InstallmentFrequency = request.InstallmentFrequency;
    liability.EndDate = request.EndDate;
    liability.Pillar = request.Pillar;
    liability.Notes = request.Notes;

    await _context.SaveChangesAsync();

    return liability;
  }

  public async Task UpdateRemainingAmountAsync(Guid liabilityId, decimal payedAmount) =>
    await _context.Liabilities
      .Where(l => l.Id == liabilityId)
      .ExecuteUpdateAsync(s => s
        .SetProperty(l => l.RemainingAmount, l => l.RemainingAmount - payedAmount)
        .SetProperty(l => l.UpdatedAt, DateTimeOffset.UtcNow)
      );

  public async Task<bool> DeleteAsync(Guid id)
  {
    var liability = await GetByIdAsync(id);

    if (liability is null)
    {
      return false;
    }

    // le Transactions collegate diventano normali per via del DeleteBehaviour.SetNull
    _context.Liabilities.Remove(liability);
    await _context.SaveChangesAsync();

    return true;
  }

  

  
}