import { Routes } from '@angular/router';
import { LoginComponent } from './auth/componets/login/login.component';
import { RecordComponent } from './records/components/records/record.component';
import { authGuard } from './auth/guards/auth.guard';
import { RegisterComponent } from './auth/componets/register/register.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'records', component: RecordComponent, canActivate: [authGuard] },
    { path: '', redirectTo: 'login', pathMatch: 'full' }
];
