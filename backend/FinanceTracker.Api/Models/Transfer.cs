using FinanceTracker.Api.Models.Enums;
using FinanceTracker.Api.Models.Interfaces;

namespace FinanceTracker.Api.Models;

public class Transfer : BaseEntity, IFinancialEntry
{
  public TransferType Type { get; set; }
  public Guid FromAccountId { get; set; }
  public Guid ToAccountId { get; set; }
  public decimal Amount { get; set; }
  public string Currency { get; set; } = string.Empty;
  public DateTimeOffset Date { get; set; }
  public string Description { get; set; } = string.Empty;
  public string? Notes { get; set; }
}