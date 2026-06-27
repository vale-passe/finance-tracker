import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { CreateTransactionRequest, Transaction, TransactionFilters, TransactionSummary } from "@core/models";
import { environment } from "@env/environment";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class TransactionService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/transactions`;

  getAll(filters?: TransactionFilters): Observable<Transaction[]> {
    let params = new HttpParams();

    if (filters?.accountId) {
      params = params.set('accountId', filters.accountId);
    }

    if (filters?.from) {
      params = params.set('from', filters.from);
    }

    if (filters?.to) {
      params = params.set('to', filters.to);
    }

    if (filters?.categoryId) {
      params = params.set('categoryId', filters.categoryId);
    }

    if (filters?.type) {
      params = params.set('type', filters.type);
    }

    return this.http.get<Transaction[]>(this.baseUrl, { params });
  }

  getById(id: string): Observable<Transaction> {
    return this.http.get<Transaction>(`${this.baseUrl}/${id}`);
  }

  getSummary(): Observable<TransactionSummary> {
    return this.http.get<TransactionSummary>(`${this.baseUrl}/summary`);
  }

  create(request: CreateTransactionRequest): Observable<Transaction> {
    return this.http.post<Transaction>(this.baseUrl, request);
  }

  update(id: string, request: CreateTransactionRequest): Observable<Transaction> {
    return this.http.put<Transaction>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}