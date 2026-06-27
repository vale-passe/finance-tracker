export type TransferType = 'Regular' | 'PillarAllocation';

export interface Transfer {
  id: string;
  type: TransferType;
  fromAccountId: string;
  toAccountId: string;
  amount: number;
  currency: string;
  date: string;
  description: string;
  notes: string | null;
  createdAt: string;
  updatedAt: string;
}

export interface CreateTransferRequest {
  type: TransferType;
  fromAccountId: string;
  toAccountId: string;
  amount: number;
  currency: string;
  date: string;
  description: string;
  notes: string | null;
}