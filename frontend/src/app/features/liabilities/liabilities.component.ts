import { Component } from '@angular/core';

@Component({
  selector: 'app-liabilities',
  standalone: true,
  template: `
    <header class="page-header">
      <h1 style="font-size:22px;font-weight:600;">Debiti</h1>
    </header>
    <p style="color:var(--graphite);">In arrivo nella prossima fase.</p>
  `
})
export class LiabilitiesComponent {}