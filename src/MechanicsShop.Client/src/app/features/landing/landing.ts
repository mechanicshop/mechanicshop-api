import { Component } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatToolbar } from '@angular/material/toolbar';
import { RouterLink } from '@angular/router';

interface Service {
  icon: string;
  title: string;
  description: string;
}

@Component({
  selector: 'app-landing',
  imports: [MatButton, MatCardModule, MatIcon, MatToolbar, RouterLink],
  template: `
    <div class="h-screen flex flex-col gap-2">
      <mat-toolbar>
        <span class="text-xl font-bold" style="color: var(--mat-sys-primary)">MechanicShop</span>
        <span class="flex-1"></span>
        <button mat-flat-button color="primary" routerLink="/auth/login">Login</button>
      </mat-toolbar>

      <section class="flex-1 flex flex-col items-center justify-center px-8 gap-4">
        <h2 class="mb-10 text-center text-3xl font-bold">Our Expert Services</h2>

        <div class="grid gap-8 sm:grid-cols-2 lg:grid-cols-3 max-w-6xl">
          @for (service of services; track service.title) {
            <mat-card appearance="outlined">
              <mat-card-content class="flex flex-col items-center text-center p-8">
                <mat-icon
                  class="text-7xl"
                  style="width: auto; height: auto; color: var(--mat-sys-primary)"
                  >{{ service.icon }}</mat-icon
                >
                <h3 class="mb-3 mt-5 text-2xl font-semibold">{{ service.title }}</h3>
                <p class="text-lg">{{ service.description }}</p>
              </mat-card-content>
            </mat-card>
          }
        </div>
      </section>
    </div>
  `,
})
export class Landing {
  readonly services: Service[] = [
    {
      icon: 'build',
      title: 'General Maintenance',
      description:
        'Oil changes, tire rotations, fluid checks, and more to keep your car running smoothly.',
    },
    {
      icon: 'settings',
      title: 'Brake & Tire Services',
      description:
        'Professional brake inspections, repairs, and new tire installations for your safety.',
    },
    {
      icon: 'timeline',
      title: 'Engine Diagnostics',
      description:
        'Advanced diagnostics to accurately identify and fix any engine performance issues.',
    },
    {
      icon: 'flash_on',
      title: 'Electrical System Repair',
      description: "Resolving issues with your car's wiring, lights, battery, and charging system.",
    },
    {
      icon: 'ac_unit',
      title: 'AC & Heating Repair',
      description: "Ensuring your car's climate control system works perfectly year-round.",
    },
    {
      icon: 'search',
      title: 'Pre-Purchase Inspections',
      description: 'Thorough inspections to give you peace of mind before buying a used vehicle.',
    },
  ];
}
