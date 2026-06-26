using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.DTOs.Liabilities;

public record CreateLiabilityRequest(
  string Name,
  LiablityType Type,
  Guid AccountId,
  decimal OriginalAmount,
  decimal InterestRate,
  decimal InstallmentAmount,
  InstallmentFrequency InstallmentFrequency,
  DateTimeOffset StartDate,
  DateTimeOffset EndDate,
  Pillar Pillar,
  string? Notes
);