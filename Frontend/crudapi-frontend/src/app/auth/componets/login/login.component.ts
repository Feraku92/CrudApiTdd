import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  userName = '';
  password = '';
  error = '';

    constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login() {
    this.error = '';
    this.authService.login(this.userName, this.password)
      .subscribe({
        next: () => {
          const redrectUrl = sessionStorage.getItem('redirectUrl') || '/records';
          sessionStorage.removeItem('redirectUrl');
          this.router.navigate(['/records']);
        },
        error: (err) => {this.error = 'Invalid credentials. Please try again.';}
      });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  register(){
    this.router.navigate(['/register']);
  }

}
