import { Component, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ThemeService } from '@core/services/theme.service';

interface NavItem {
  path: string;
  label: string;
  icon: string;
}

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  readonly themeService = inject(ThemeService);

  readonly navItems: NavItem[] = [
    { path: '/dashboard',    label: 'Dashboard',     icon: '◳' },
    { path: '/accounts',     label: 'Conti',         icon: '▦' },
    { path: '/transactions', label: 'Transazioni',   icon: '↕' },
    { path: '/transfers',    label: 'Trasferimenti', icon: '⇄' },
    { path: '/categories',   label: 'Categorie',     icon: '⊞' },
    { path: '/liabilities',  label: 'Debiti',        icon: '⊟' }
  ];
}
