export type TransactionType = 'Income' | 'Expense';

export interface Transaction {
  id: string;
  type: TransactionType;
  amount: number;
  currency: string;
  date: string;
  description: string;
  accountId: string;
  categoryId: string | null;
  isCategorized: boolean;
  liabilityId: string | null;
  notes: string | null;
  createdAt: string;
  updatedAt: string;
}

export interface CreateTransactionRequest {
  type: TransactionType;
  amount: number;
  currency: string;
  date: string;
  description: string;
  accountId: string;
  categoryId: string | null;
  liabilityId: string | null;
  notes: string | null;
}

export interface TransactionFilters {
  from?: string;
  to?: string;
  accountId?: string;
  categoryId?: string;
  type?: TransactionType;
}

export interface TransactionSummary {
  totalIncome: number;
  totalExpenses: number;
  balance: number;
}