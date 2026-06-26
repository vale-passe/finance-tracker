using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.DTOs.Transfers;

public record TransferResponse(
  Guid Id,
  TransferType Type,
  Guid FromAccountId,
  Guid ToAccountId,
  decimal Amount,
  string Currency,
  DateTimeOffset Date,
  string Description,
  string? Notes,
  DateTimeOffset CreatedAt,
  DateTimeOffset UpdatedAt
);