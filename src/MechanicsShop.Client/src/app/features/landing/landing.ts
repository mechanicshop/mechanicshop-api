import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink } from '@angular/router';

interface Service {
  icon: string;
  title: string;
  description: string;
}

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [MatButtonModule, MatCardModule, MatIcon, MatToolbarModule, RouterLink],
  template: `
    <div class="landing-container">
      <mat-toolbar class="landing-navbar">
        <div class="container navbar-content">
          <div class="brand-logo">
            <mat-icon class="brand-icon">handyman</mat-icon>
            <span class="logo-text">MechanicShop</span>
          </div>
          <button class="nav-login-btn" mat-flat-button color="primary" routerLink="/auth/login">
            <mat-icon>login</mat-icon>
            Dashboard Login
          </button>
        </div>
      </mat-toolbar>

      <header class="hero-section">
        <div class="container hero-content">
          <div class="hero-text-block">
            <span class="hero-badge">Professional Auto Care</span>
            <h1 class="hero-title">
              Keep Your Vehicle Running <br />
              <span class="brand-highlight">At Its Absolute Best</span>
            </h1>
            <p class="hero-subtitle">
              Manage operations, track repairs, and schedule services with our unified shop portal.
              Fast, reliable, and premium quality service guaranteed.
            </p>
            <div class="hero-actions">
              <button class="hero-cta-btn" mat-flat-button color="primary" routerLink="/auth/login">
                Get Started
                <mat-icon>arrow_forward</mat-icon>
              </button>
              <a class="hero-link-btn" href="#services">Our Services</a>
            </div>
          </div>
        </div>
      </header>

      <section class="services-section container" id="services">
        <div class="section-header">
          <h2 class="section-title">Our Expert Services</h2>
          <p class="section-subtitle">
            We provide full-service mechanical care, diagnostics, and repairs for all makes and
            models.
          </p>
        </div>

        <div class="services-grid">
          @for (service of services; track service.title) {
            <mat-card class="service-card" appearance="outlined">
              <mat-card-content class="service-card-content">
                <div class="card-header-row">
                  <div class="icon-container">
                    <mat-icon class="service-icon">{{ service.icon }}</mat-icon>
                  </div>
                  <h3 class="service-title">{{ service.title }}</h3>
                </div>
                <p class="service-desc">{{ service.description }}</p>
              </mat-card-content>
            </mat-card>
          }
        </div>
      </section>

      <footer class="landing-footer">
        <div class="container footer-content">
          <p>&copy; 2026 MechanicShop. All rights reserved.</p>
        </div>
      </footer>
    </div>
  `,
  styles: `
    .landing-container {
      min-height: 100vh;
      display: flex;
      flex-direction: column;
      background-color: var(--color-bg);
      color: var(--color-ink);
    }
    .landing-navbar {
      background-color: rgba(255, 255, 255, 0.8) !important;
      backdrop-filter: blur(10px);
      border-bottom: 1px solid var(--color-outline-variant);
      position: sticky;
      top: 0;
      z-index: 100;
      height: 64px;
    }
    .navbar-content {
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: 100%;
    }
    .brand-logo {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }
    .brand-icon {
      color: var(--color-primary);
      width: 24px;
      height: 24px;
      font-size: 24px;
    }
    .logo-text {
      font-size: 1.25rem;
      font-weight: 800;
      color: var(--color-ink);
      letter-spacing: -0.015em;
    }
    .nav-login-btn {
      border-radius: var(--radius-md);
      font-weight: 500;
      height: 40px;
    }

    /* Hero Section */
    .hero-section {
      background: linear-gradient(180deg, rgba(255, 152, 0, 0.04) 0%, rgba(255, 255, 255, 0) 100%);
      padding-block: 5rem 4rem;
      border-bottom: 1px solid var(--color-outline-variant);
    }
    .hero-content {
      display: flex;
      justify-content: center;
      text-align: center;
    }
    .hero-text-block {
      max-width: 800px;
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 1.5rem;
    }
    .hero-badge {
      display: inline-block;
      padding: 0.35rem 1rem;
      background-color: rgba(255, 152, 0, 0.08);
      color: var(--color-primary);
      border-radius: var(--radius-full);
      font-size: 0.75rem;
      font-weight: 700;
      text-transform: uppercase;
      letter-spacing: 0.05em;
    }
    .hero-title {
      font-size: 2.75rem;
      font-weight: 800;
      line-height: 1.15;
      color: var(--color-ink);
      letter-spacing: -0.02em;
    }
    .brand-highlight {
      color: var(--color-primary);
      background: linear-gradient(120deg, var(--color-primary) 0%, rgba(255, 152, 0, 0.8) 100%);
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
    }
    .hero-subtitle {
      font-size: 1.125rem;
      color: var(--color-muted);
      line-height: 1.6;
      max-width: 600px;
    }
    .hero-actions {
      display: flex;
      align-items: center;
      gap: 1.25rem;
      margin-top: 0.5rem;
    }
    .hero-cta-btn {
      height: 48px;
      border-radius: var(--radius-md);
      font-size: 1rem;
      font-weight: 600;
      padding-inline: 1.5rem;
    }
    .hero-link-btn {
      color: var(--color-ink);
      font-weight: 600;
      text-decoration: none;
      transition: color 0.2s ease;
      font-size: 0.95rem;

      &:hover {
        color: var(--color-primary);
      }
    }

    /* Services Section */
    .services-section {
      padding-block: 4rem 5rem;
    }
    .section-header {
      text-align: center;
      margin-bottom: 3.5rem;
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 0.75rem;
    }
    .section-title {
      font-size: 2rem;
      font-weight: 800;
      color: var(--color-ink);
      letter-spacing: -0.015em;
      margin: 0;
    }
    .section-subtitle {
      font-size: 1rem;
      color: var(--color-muted);
      max-width: 500px;
      line-height: 1.5;
      margin: 0;
    }
    .services-grid {
      display: grid;
      gap: 1.5rem;
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

    .service-card {
      background-color: var(--color-surface);
      border: 1px solid var(--color-outline-variant);
      border-top: 3px solid var(--color-primary);
      border-radius: var(--radius-md);
      transition:
        transform 0.2s var(--ease-standard),
        box-shadow 0.2s var(--ease-standard);

      &:hover {
        transform: translateY(-4px);
        box-shadow: var(--shadow-md);
      }
    }
    .service-card-content {
      padding: 1.5rem;
      display: flex;
      flex-direction: column;
      gap: 0.75rem;
    }
    .card-header-row {
      display: flex;
      align-items: center;
      gap: 0.75rem;
    }
    .icon-container {
      width: 2.25rem;
      height: 2.25rem;
      border-radius: var(--radius-sm);
      background-color: rgba(255, 152, 0, 0.08);
      display: flex;
      align-items: center;
      justify-content: center;
      flex-shrink: 0;
    }
    .service-icon {
      color: var(--color-primary);
      font-size: 1.25rem;
      width: 20px;
      height: 20px;
      display: flex;
      align-items: center;
      justify-content: center;
    }
    .service-title {
      font-size: 1rem;
      font-weight: 700;
      color: var(--color-ink);
      margin: 0;
      text-transform: uppercase;
      letter-spacing: 0.05em;
    }
    .service-desc {
      font-size: 0.875rem;
      color: var(--color-muted);
      line-height: 1.5;
      margin: 0;
    }

    .landing-footer {
      background-color: var(--color-surface);
      border-top: 1px solid var(--color-outline-variant);
      padding-block: 1.5rem;
      margin-top: auto;
      text-align: center;
    }
    .footer-content p {
      font-size: 0.8125rem;
      color: var(--color-muted);
      margin: 0;
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
