namespace FinanceTracker.Api.Models;

// classe base astratta per TPC (Table Per Concrete Type):
// una tabella separata per ogni classe concreta che estende BaseEntity,
// senza colonne discriminatore e senza JOIN aggiuntivi
public class BaseEntity
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
}