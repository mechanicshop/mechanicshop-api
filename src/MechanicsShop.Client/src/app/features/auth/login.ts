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
    <div class="h-screen flex items-center justify-center bg-gray-50">
      <mat-card class="w-full max-w-md p-6 gap-4">
        <mat-card-header class="justify-center mb-6">
          <mat-card-title class="text-3xl font-bold text-center">Login</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form class="flex flex-col gap-4" [formGroup]="loginForm" (ngSubmit)="onSubmit()">
            <mat-form-field class="w-full" appearance="outline">
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

            <mat-form-field class="w-full" appearance="outline">
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
              class="w-full py-2 text-lg"
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
  styles: [],
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
