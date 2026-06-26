using FinanceTracker.Api.Models.Enums;

namespace FinanceTracker.Api.Models;

public class Account : BaseEntity
{
  public string Name { get; set; } = string.Empty;
  public AccountType Type { get; set; }
  public Pillar? Pillar { get; set; } // Investment e Crypto non hanno pilastro fisso - lo hanno le Holdings
  public string Currency { get; set; } = string.Empty;
  public decimal Balance { get; set; } // snapshot aggiornato automaticamente
  public string Color { get; set; } = string.Empty;
  public bool IsAchived { get; set; } // conto chiuso, senza perdere la storia
  public string? Notes { get; set; }
}