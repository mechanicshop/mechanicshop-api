import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  NonNullableFormBuilder,
  ReactiveFormsModule,
} from '@angular/forms';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { PageHeader } from '@shared/components/page-header';

// import { ScheduleStore } from './schedules.store';

interface ScheduleFormGroup {
  date: FormControl<Date | string | null>;
}

@Component({
  selector: 'app-schedules',
  standalone: true,
  imports: [
    PageHeader,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    ReactiveFormsModule,
  ],
  providers: [provideNativeDateAdapter()],
  template: `
    <div>
      <main class="container page-layout">
        <div class="header">
          <app-page-header>
            <h1>Daily Schedule</h1>
            <p>Schedules feature coming soon.</p>
          </app-page-header>
        </div>
        <div class="content">
          <form [formGroup]="scheduleForm">
            <mat-form-field class="filter-field date-field" appearance="outline">
              <mat-label>Choose a date</mat-label>
              <input [matDatepicker]="picker" formControlName="date" matInput />
              <mat-datepicker-toggle [for]="picker" matIconSuffix></mat-datepicker-toggle>
              <mat-datepicker #picker></mat-datepicker>
            </mat-form-field>
          </form>
        </div>
      </main>
    </div>
  `,
  styles: `
    .page-layout {
      padding-block: 2rem;
      display: flex;
    }

    .content {
      margin-top: 1.5rem;
    }

    .date-field {
      max-width: 220px;
    }
  `,
})
export class Schedules {
  private readonly fb = inject(NonNullableFormBuilder);

  protected readonly scheduleForm: FormGroup<ScheduleFormGroup> = this.fb.group<ScheduleFormGroup>({
    date: this.fb.control<Date | string | null>(new Date()),
  });

  protected get dateControl() {
    return this.scheduleForm.controls.date;
  }
}
