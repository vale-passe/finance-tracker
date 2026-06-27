import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  template: `
    <header class="page-header">
      <h1 style="font-size:22px;font-weight:600;">Dashboard</h1>
    </header>
    <p style="color:var(--graphite);">
      Qui arriverà la panoramica generale: saldo, grafici, ultimi movimenti.
    </p>
  `
})
export class DashboardComponent {}