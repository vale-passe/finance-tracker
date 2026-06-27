export type AccountType =
  'Checking' | 'Savings' | 'Cash' | 'CreditCard' | 'Investment' | 'Crypto' | 'PensionFund';

export type Pillar = 'Protection' | 'Pension' | 'Accumulation' | 'Lifestyle';

export interface Account {
  id: string;
  name: string;
  type: AccountType;
  pillar: Pillar | null;
  currency: string;
  balance: number;
  color: string;
  isArchived: boolean;
  notes: string | null;
  createdAt: string;
  updatedAt: string;
}

export interface CreateAccountRequest {
  name: string;
  type: AccountType;
  pillar: Pillar | null;
  currency: string;
  color: string;
  notes: string | null;
}