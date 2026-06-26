using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.DTOs.Transfers;

public record CreateTransferRequest(
  TransferType Type,
  Guid FromAccountId,
  Guid ToAccountId,
  decimal Amount,
  string Currency,
  DateTimeOffset Date,
  string Description,
  string? Notes
);