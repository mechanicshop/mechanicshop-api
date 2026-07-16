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
    <div class="landing-container">
      <mat-toolbar>
        <div class="container landing-toolbar-content">
          <span class="logo-text" style="color: var(--mat-sys-primary); font-size: 1.25rem;">MechanicShop</span>
          <button mat-flat-button color="primary" routerLink="/auth/login">Login</button>
        </div>
      </mat-toolbar>

      <section class="landing-hero">
        <h2 class="landing-heading">Our Expert Services</h2>

        <div class="services-grid">
          @for (service of services; track service.title) {
            <mat-card appearance="outlined">
              <mat-card-content class="service-card-content">
                <mat-icon
                  class="service-icon"
                  style="width: auto; height: auto; color: var(--mat-sys-primary)"
                  >{{ service.icon }}</mat-icon
                >
                <h3 class="service-title">{{ service.title }}</h3>
                <p class="service-desc">{{ service.description }}</p>
              </mat-card-content>
            </mat-card>
          }
        </div>
      </section>
    </div>
  `,
  styles: `
    .landing-toolbar-content {
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: 100%;
    }
    .landing-container {
      height: 100vh;
      display: flex;
      flex-direction: column;
      gap: 0.5rem;
    }
    .landing-hero {
      flex: 1;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding-inline: 2rem;
      gap: 1rem;
    }
    .landing-heading {
      margin-bottom: 2.5rem;
      text-align: center;
      font-size: 1.875rem;
      font-weight: 700;
    }
    .services-grid {
      display: grid;
      gap: 2rem;
      max-width: 1152px;
      width: 100%;
    }
    @media (min-width: 640px) {
      .services-grid {
        grid-template-columns: repeat(2, 1fr);
      }
    }
    @media (min-width: 1024px) {
      .services-grid {
        grid-template-columns: repeat(3, 1fr);
      }
    }
    .service-card-content {
      display: flex;
      flex-direction: column;
      align-items: center;
      text-align: center;
      padding: 2rem;
    }
    .service-icon {
      font-size: 4.5rem;
    }
    .service-title {
      margin-bottom: 0.75rem;
      margin-top: 1.25rem;
      font-size: 1.5rem;
      font-weight: 600;
    }
    .service-desc {
      font-size: 1.125rem;
    }
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
