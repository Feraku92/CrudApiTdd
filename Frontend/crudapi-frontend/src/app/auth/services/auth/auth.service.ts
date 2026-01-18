import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpParams  } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import sha256 from 'crypto-js/sha256';

@Injectable({
  providedIn: 'root',
})

export class AuthService {
  private readonly apiUserUrl = `${environment.apiUrl}/User`;

  constructor(private http: HttpClient) {}

  login(userName: string, password: string) {
    const hashedPassword = this.hashPassword(password);
    return this.http.post<{ token: string }>(
      `${this.apiUserUrl}/login`,
      { userName, password: hashedPassword }
    ).pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
      })
    );
  }

  private hashPassword(password: string): string {
    return sha256(password).toString();
  }

  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  register(userName: string, email: string, password: string) {
    const hashedPassword = this.hashPassword(password);
    return this.http.post(
      `${this.apiUserUrl}/register`,
      { userName, email, password: hashedPassword }
    );
  }

}
