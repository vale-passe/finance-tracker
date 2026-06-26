using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.DTOs.Transactions;

public record CreateTransactionRequest(
  TransactionType Type,
  decimal Amount,
  string Currency,
  DateTimeOffset Date,
  string Description,
  Guid AccountId,
  Guid? CategoryId,
  Guid? LiabilityId, // null = transazione normale, valorizzato = rata di un debito
  string? Notes
);