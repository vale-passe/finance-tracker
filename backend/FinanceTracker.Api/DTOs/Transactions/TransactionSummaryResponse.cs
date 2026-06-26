namespace FinanceTracker.Api.DTOs.Transactions;

public record TransactionSummaryResponse(
  decimal TotalIncome,
  decimal TotalExpenses,
  decimal Balance
);