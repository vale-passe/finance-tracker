using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.DTOs.Liabilities;

public record LiabiltyResponse(
  Guid Id,
  string Name,
  LiablityType Type,
  Guid AccountId,
  decimal OriginalAmount,
  decimal RemainingAmount,
  decimal InterestRate,
  decimal InstallmentAmount,
  InstallmentFrequency InstallmentFrequency,
  DateTimeOffset StartDate,
  DateTimeOffset EndDate,
  Pillar Pillar,
  string? Notes,
  DateTimeOffset CreatedAt,
  DateTimeOffset UpdatedAt
);