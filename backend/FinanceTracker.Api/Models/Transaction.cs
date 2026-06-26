using FinanceTracker.Api.Models.Enums;
using FinanceTracker.Api.Models.Interfaces;

namespace FinanceTracker.Api.Models;

public class Transaction : BaseEntity, IFinancialEntry
{
  public TransactionType Type { get; set; }
  public decimal Amount { get; set; }
  public string Currency { get; set; } = string.Empty;
  public DateTimeOffset Date { get; set; }
  public string Description { get; set; } = string.Empty;
  public Guid AccountId { get; set; }
  public Guid? CategoryId { get; set; } // null = non ancora categorizzata
  public bool IsCategorized { get; set; } // false = appare nell'inbox
  public Guid? LiabilityId { get; set; } // null = transazione normale, valorizzato = rata di un debito
  public string? Notes { get; set; }
}