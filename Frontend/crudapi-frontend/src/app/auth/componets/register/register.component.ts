import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-register.component',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  userName = '';
  email = '';
  password = '';
  error: string = '';
  success: string = '';

    constructor(
    private authService: AuthService,
    private router: Router
  ) {}

    register() {
    this.authService.register(this.userName, this.email, this.password)
      .subscribe({
        next: () => this.router.navigate(['/login']),
        error: () => alert('Error registering user')
      });
  }
  login(){
    this.router.navigate(['/login']);
  }
}
