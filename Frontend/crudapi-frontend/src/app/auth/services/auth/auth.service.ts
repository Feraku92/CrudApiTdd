import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})

export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/User`;

  constructor(private http: HttpClient) {}

  login(userName: string, password: string) {
    console.log(this.apiUrl);
    return this.http.post<{ token: string }>(
      `${this.apiUrl}/login`,
      { userName, password }
    ).pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
      })
    );
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
    return this.http.post(
      `${this.apiUrl}/register`,
      { userName, email, password }
    );
  }

}
