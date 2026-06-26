using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.Models;

public class Liability : BaseEntity
{
  public string Name { get; set; } = string.Empty;
  public LiablityType Type { get; set; }
  public Guid AccountId { get; set; } // conto da cui escono le rate
  public decimal OriginalAmount { get; set; } // debito originale
  public decimal RemainingAmount { get; set; } // capitale residuo (snapshot)
  public decimal InterestRate { get; set; } // tasso annuo nominale (es. 0.035)
  public decimal InstallmentAmount { get; set; } // importo rata
  public InstallmentFrequency InstallmentFrequency { get; set; }
  public DateTimeOffset StartDate { get; set; }
  public DateTimeOffset EndDate { get; set; }
  public Pillar Pillar { get; set; }
  public string? Notes { get; set; }
}