using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.DTOs.Liabilities;

public record UpdateLiabilityRequest(
  string Name,
  decimal InterestRate,
  decimal InstallmentAmount,
  InstallmentFrequency InstallmentFrequency,
  DateTimeOffset EndDate,
  Pillar Pillar,
  string? Notes
);