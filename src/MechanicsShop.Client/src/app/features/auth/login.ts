import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

import { AuthService } from './auth.service';

interface LoginFormGroup {
  email: FormControl<string>;
  password: FormControl<string>;
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
  ],
  template: `
    <div class="login-container">
      <mat-card class="login-card">
        <mat-card-header class="login-header">
          <mat-card-title class="login-title">Login</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form class="login-form" [formGroup]="loginForm" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline">
              <mat-label>Email</mat-label>
              <input
                matInput
                type="email"
                formControlName="email"
                placeholder="Enter your email"
                required
              />
              @if (emailControl.hasError('required') && emailControl.touched) {
                <mat-error>Email is required</mat-error>
              }
              @if (emailControl.hasError('email') && emailControl.touched) {
                <mat-error>Please enter a valid email address</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Password</mat-label>
              <input
                matInput
                type="password"
                formControlName="password"
                placeholder="Enter your password"
                required
              />
              @if (passwordControl.hasError('required') && passwordControl.touched) {
                <mat-error>Password is required</mat-error>
              }
            </mat-form-field>

            <button
              class="login-button"
              [disabled]="loginForm.invalid"
              mat-flat-button
              color="primary"
              type="submit"
            >
              Sign In
            </button>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .login-container {
      height: 100vh;
      width: 100%;
      display: flex;
      align-items: center;
      justify-content: center;
      background-color: #f9fafb;
    }
    .login-card {
      padding: 1.5rem;
      display: flex;
      flex-direction: column;
      gap: 1rem;
      background-color: #ffffff;
      box-shadow: var(--shadow-xl);
      border-radius: var(--radius-lg);
      max-width: 420px;
      width: 100%;
    }
    .login-header {
      justify-content: center;
      margin-bottom: 1.5rem;
    }
    .login-title {
      font-size: 1.875rem;
      font-weight: 700;
      text-align: center;
    }
    .login-form {
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }
    .login-form mat-form-field {
      width: 100%;
    }
    .login-button {
      width: 100%;
      padding-block: 0.5rem;
      font-size: 1.125rem;
    }
  `,
})
export class Login {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly loginForm: FormGroup<LoginFormGroup> = this.fb.group<LoginFormGroup>({
    email: this.fb.control('', [Validators.required, Validators.email]),
    password: this.fb.control('', [Validators.required]),
  });

  protected get emailControl() {
    return this.loginForm.controls.email;
  }

  protected get passwordControl() {
    return this.loginForm.controls.password;
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.getRawValue();
      this.authService.login({ email, password }).subscribe({
        next: () => {
          this.router.navigate(['/dashboard']);
        },
        error: () => {
          this.snackBar.open('Invalid email or password. Please try again.', 'Close', {
            duration: 4000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
          });
        },
      });
    }
  }
}
