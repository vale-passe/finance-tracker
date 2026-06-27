import { Component, computed } from '@angular/core';
import { EuroPipe } from '@shared/pipes/euro.pipe';
import { Account } from '@core/models';
import { environment } from '@env/environment';
import { httpResource } from '@angular/common/http';

@Component({
  selector: 'app-accounts',
  imports: [EuroPipe],
  template: `
    <header class="page-header">
      <div>
        <h1 class="page-title">Conti</h1>
        <p class="page-subtitle">Tutti i tuoi conti in un colpo d'occhio</p>
      </div>
      <button class="btn-primary" (click)="onCreate()">Nuovo conto</button>
    </header>

    @if (accounts.isLoading()) {
      <p class="state-msg">Caricamento conti…</p>
    }

    @if (accounts.error()) {
      <p class="state-msg state-msg--error">
        Impossibile caricare i conti. Verifica che il backend sia in esecuzione.
      </p>
    }

    @if (accounts.value(); as list) {

      <!-- riepilogo patrimonio -->
      <div class="summary">
        <span class="summary__label">Patrimonio totale</span>
        <span class="summary__value amount">{{ totalBalance() | euro }}</span>
      </div>

      <!-- lista conti stile registro -->
      <div class="ledger">
        @for (account of list; track account.id) {
          <div class="ledger__row">
            <span
              class="ledger__dot"
              [style.background]="pillarColor(account.pillar)">
            </span>

            <div class="ledger__info">
              <span class="ledger__name">{{ account.name }}</span>
              <span class="ledger__meta">
                {{ accountTypeLabel(account.type) }}
                @if (account.pillar) { · {{ pillarLabel(account.pillar) }} }
              </span>
            </div>

            <span
              class="ledger__amount amount"
              [class.amount--income]="account.balance > 0"
              [class.amount--expense]="account.balance < 0">
              {{ account.balance | euro }}
            </span>
          </div>
        } @empty {
          <div class="empty">
            <p>Nessun conto ancora.</p>
            <button class="btn-primary" (click)="onCreate()">
              Crea il primo conto
            </button>
          </div>
        }
      </div>
    }
  `,
  styles: [`
    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 28px;
    }

    .page-title {
      font-size: 22px;
      font-weight: 600;
      color: var(--ink);
    }

    .page-subtitle {
      font-size: 14px;
      color: var(--graphite);
      margin-top: 2px;
    }

    .btn-primary {
      background: var(--accent);
      color: var(--accent-text);
      border: none;
      padding: 9px 16px;
      border-radius: var(--radius);
      font-family: var(--font-sans);
      font-size: 14px;
      font-weight: 500;
      cursor: pointer;
      transition: opacity 0.15s ease;
    }

    .btn-primary:hover { opacity: 0.9; }

    .summary {
      display: flex;
      flex-direction: column;
      gap: 4px;
      padding: 18px 20px;
      background: var(--surface);
      border: 1px solid var(--hairline);
      border-radius: var(--radius-card);
      margin-bottom: 20px;
    }

    .summary__label {
      font-size: 11px;
      letter-spacing: 0.08em;
      text-transform: uppercase;
      color: var(--graphite);
    }

    .summary__value {
      font-size: 28px;
      font-weight: 500;
      color: var(--ink);
    }

    .ledger {
      background: var(--surface);
      border: 1px solid var(--hairline);
      border-radius: var(--radius-card);
      overflow: hidden;
    }

    .ledger__row {
      display: flex;
      align-items: center;
      gap: 14px;
      padding: 14px 20px;
      border-bottom: 1px solid var(--hairline);
    }

    .ledger__row:last-child { border-bottom: none; }

    .ledger__dot {
      width: 9px;
      height: 9px;
      border-radius: 50%;
      flex-shrink: 0;
    }

    .ledger__info {
      display: flex;
      flex-direction: column;
      gap: 1px;
      flex: 1;
    }

    .ledger__name {
      font-size: 14px;
      font-weight: 500;
      color: var(--ink);
    }

    .ledger__meta {
      font-size: 12px;
      color: var(--graphite);
    }

    .ledger__amount {
      font-size: 15px;
      font-weight: 500;
    }

    .state-msg {
      padding: 40px;
      text-align: center;
      color: var(--graphite);
    }

    .state-msg--error { color: var(--vermilion); }

    .empty {
      padding: 48px 20px;
      text-align: center;
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 16px;
      color: var(--graphite);
    }
  `]
})
export class AccountsComponent {
  // httpResource carica automaticamente i dati e gestisce loading/error
  accounts = httpResource<Account[]>(
    () => `${environment.apiUrl}/accounts`
  );

  // computed — il totale si ricalcola automaticamente quando i conti cambiano
  totalBalance = computed(() => {
    const list = this.accounts.value() ?? [];
    return list
      .filter(a => !a.isArchived)
      .reduce((sum, a) => sum + a.balance, 0);
  });

  onCreate(): void {
    // dialog di creazione implementato in Fase 5 con PrimeNG
    console.log('apri creazione conto');
  }

  pillarColor(pillar: string | null): string {
    const colors: Record<string, string> = {
      Protection: 'var(--pillar-protection)',
      Pension: 'var(--pillar-pension)',
      Accumulation: 'var(--pillar-accumulation)',
      Lifestyle: 'var(--pillar-lifestyle)'
    };
    return pillar ? colors[pillar] ?? 'var(--graphite)' : 'var(--graphite)';
  }

  pillarLabel(pillar: string): string {
    const labels: Record<string, string> = {
      Protection: 'Protezione',
      Pension: 'Previdenza',
      Accumulation: 'Accumulo',
      Lifestyle: 'Stile di vita'
    };
    return labels[pillar] ?? pillar;
  }

  accountTypeLabel(type: string): string {
    const labels: Record<string, string> = {
      Checking: 'Conto corrente',
      Savings: 'Conto deposito',
      Cash: 'Contanti',
      CreditCard: 'Carta di credito',
      Investment: 'Investimenti',
      Crypto: 'Crypto',
      PensionFund: 'Fondo pensione'
    };
    return labels[type] ?? type;
  }
}