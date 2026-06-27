import { Component } from '@angular/core';

@Component({
  selector: 'app-transactions',
  standalone: true,
  template: `
    <header class="page-header">
      <div>
        <h1 style="font-size:22px;font-weight:600;">Transazioni</h1>
      </div>
    </header>
    <p style="color:var(--graphite);">In arrivo nella prossima fase.</p>
  `
})
export class TransactionsComponent {}