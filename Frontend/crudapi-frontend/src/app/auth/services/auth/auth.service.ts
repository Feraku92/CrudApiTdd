import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpParams  } from '@angular/common/http';
import { max, Observable, tap } from 'rxjs';
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
        this.saveToken(response.token);
      })
    );
  }

  private hashPassword(password: string): string {
    return sha256(password).toString();
  }

  saveToken(token: string): void {
    localStorage.setItem('token', token);
    localStorage.setItem('tokenTimestamp', Date.now().toString());
  }

  getToken(): string | null {
    const token = localStorage.getItem('token');
    const timestamp = localStorage.getItem('tokenTimestamp');

    if (token && timestamp){
      const tokenAge = Date.now() - parseInt(timestamp);
      const maxAge = 24 * 60 * 60 * 1000;

      if (tokenAge>maxAge){
        this.logout();
        return null;
      }

      return token;
    }
    return null;
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('tokenTimestamp');
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
