import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { Liability, CreateLiabilityRequest } from '@core/models';

@Injectable({ providedIn: 'root' })
export class LiabilityService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/liabilities`;

  getAll(): Observable<Liability[]> {
    return this.http.get<Liability[]>(this.baseUrl);
  }

  getById(id: string): Observable<Liability> {
    return this.http.get<Liability>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateLiabilityRequest): Observable<Liability> {
    return this.http.post<Liability>(this.baseUrl, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}