namespace FinanceTracker.Api.Models.Interfaces;

// interfaccia condivisa tra Transcation e Transfer - non cambia nulla a DB (EF Core la ignora)
// permette però di scrivere logica generica su entrambi i tipi
// es. un report che aggrega entrate e trasferimenti per data
public interface IFinancialEntry
{
  decimal Amount { get; set; }
  string Currency { get; set; }
  DateTimeOffset Date { get; set; }
  string Description { get; set; }
  string? Notes { get; set; }
}