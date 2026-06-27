import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { Transfer, CreateTransferRequest } from '@core/models';

@Injectable({ providedIn: 'root' })
export class TransferService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/transfers`;

  getAll(): Observable<Transfer[]> {
    return this.http.get<Transfer[]>(this.baseUrl);
  }

  getById(id: string): Observable<Transfer> {
    return this.http.get<Transfer>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateTransferRequest): Observable<Transfer> {
    return this.http.post<Transfer>(this.baseUrl, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}