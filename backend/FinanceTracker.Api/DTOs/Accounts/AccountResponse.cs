using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.DTOs.Accounts;

public record AccountResponse(
  Guid Id,
  string Name,
  AccountType Type,
  Pillar? Pillar,
  string Currency,
  decimal Balance,
  string Color,
  bool IsAchived,
  string? Notes,
  DateTimeOffset CreatedAt,
  DateTimeOffset UpdatedAt
);