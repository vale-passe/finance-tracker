using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.DTOs.Accounts;

public record CreateAccountRequest(
  string Name,
  AccountType Type,
  Pillar? Pillar,
  string Currency,
  string Color,
  string? Notes
);