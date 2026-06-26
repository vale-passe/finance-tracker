using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.DTOs.Transactions;

public record TransactionResponse(
Guid Id,
TransactionType Type,
decimal Amount,
string Currency,
DateTimeOffset Date,
string Description,
Guid AccountId,
Guid? CategoryId,
Guid? LiabilityId,
bool IsCategorized,
string? Notes,
DateTimeOffset CreatedAt,
DateTimeOffset UpdatedAt
);