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
    console.log("se logeo");
    this.authService.login(this.userName, this.password)
      .subscribe({
        next: () => this.router.navigate(['/records']),
        error: () => alert('Invalid credentials')
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
