namespace FinanceTracker.Api.Models.Enums;

public enum AuditSource
{
  Manual,   // azione diretta dell'utente
  Import,   // importazione da fonte esterna
  System    // operazione automatica (es. staking reward)
}