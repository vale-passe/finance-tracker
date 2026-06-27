import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Account, CreateAccountRequest } from "@core/models";
import { environment } from "@env/environment";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class AccountService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/accounts`;

  getAll(): Observable<Account[]> {
    return this.http.get<Account[]>(this.baseUrl);
  }

  getById(id: string): Observable<Account> {
    return this.http.get<Account>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateAccountRequest): Observable<Account> {
    return this.http.post<Account>(this.baseUrl, request);
  }

  update(id: string, request: CreateAccountRequest): Observable<Account> {
    return this.http.put<Account>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}