using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.DTOs.Transactions;

// [AsParameters] negli endpoint lega i parametri della query string
// ai campi di questo record automaticamente:
// GET /api/transactions?type=Expense&accountId=... funziona senza configurazione aggiuntiva
public record TransactionFilters(
  DateTimeOffset? From = null,
  DateTimeOffset? To = null,
  Guid? AccountId = null,
  Guid? CategoryId = null,
  TransactionType? Type = null
);