export type LiabilityType =
  | 'Mortgage' | 'PersonalLoan' | 'CarLoan'
  | 'StudentLoan' | 'CreditCard' | 'Other';

export type InstallmentFrequency = 'Monthly' | 'Quarterly' | 'Yearly';

export interface Liability {
  id: string;
  name: string;
  type: LiabilityType;
  accountId: string;
  originalAmount: number;
  remainingAmount: number;
  interestRate: number;
  installmentAmount: number;
  installmentFrequency: InstallmentFrequency;
  startDate: string;
  endDate: string;
  pillar: string;
  notes: string | null;
  createdAt: string;
  updatedAt: string;
}

export interface CreateLiabilityRequest {
  name: string;
  type: LiabilityType;
  accountId: string;
  originalAmount: number;
  interestRate: number;
  installmentAmount: number;
  installmentFrequency: InstallmentFrequency;
  startDate: string;
  endDate: string;
  pillar: string;
  notes: string | null;
}