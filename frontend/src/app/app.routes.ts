import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./features/dashboard/dashboard.component')
        .then(m => m.DashboardComponent)
  },
  {
    path: 'accounts',
    loadComponent: () =>
      import('./features/accounts/accounts.component')
        .then(m => m.AccountsComponent)
  },
  {
    path: 'transactions',
    loadComponent: () =>
      import('./features/transactions/transactions.component')
        .then(m => m.TransactionsComponent)
  },
  {
    path: 'transfers',
    loadComponent: () =>
      import('./features/transfers/transfers.component')
        .then(m => m.TransfersComponent)
  },
  {
    path: 'categories',
    loadComponent: () =>
      import('./features/categories/categories.component')
        .then(m => m.CategoriesComponent)
  },
  {
    path: 'liabilities',
    loadComponent: () =>
      import('./features/liabilities/liabilities.component')
        .then(m => m.LiabilitiesComponent)
  },
  { path: '**', redirectTo: 'dashboard' }
];