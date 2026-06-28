import { Component } from '@angular/core';

@Component({
  selector: 'app-landing',
  template: `
    <header class="flex items-center justify-between px-8 py-4">
      <h1 class="text-2xl font-bold tracking-tight">MechanicShop</h1>
      <button class="rounded-md bg-black px-5 py-2 text-sm font-medium text-white">Login</button>
    </header>

    <section class="mx-auto max-w-6xl px-8 py-16">
      <h2 class="mb-12 text-center text-3xl font-bold">Our Expert Services</h2>
      <div class="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
        <div class="rounded-lg border p-6 shadow-sm">
          <h3 class="mb-2 text-lg font-semibold">General Maintenance</h3>
          <p class="text-sm leading-relaxed text-gray-600">
            Oil changes, tire rotations, fluid checks, and more to keep your car running smoothly.
          </p>
        </div>
        <div class="rounded-lg border p-6 shadow-sm">
          <h3 class="mb-2 text-lg font-semibold">Brake & Tire Services</h3>
          <p class="text-sm leading-relaxed text-gray-600">
            Professional brake inspections, repairs, and new tire installations for your safety.
          </p>
        </div>
        <div class="rounded-lg border p-6 shadow-sm">
          <h3 class="mb-2 text-lg font-semibold">Engine Diagnostics</h3>
          <p class="text-sm leading-relaxed text-gray-600">
            Advanced diagnostics to accurately identify and fix any engine performance issues.
          </p>
        </div>
        <div class="rounded-lg border p-6 shadow-sm">
          <h3 class="mb-2 text-lg font-semibold">Electrical System Repair</h3>
          <p class="text-sm leading-relaxed text-gray-600">
            Resolving issues with your car's wiring, lights, battery, and charging system.
          </p>
        </div>
        <div class="rounded-lg border p-6 shadow-sm">
          <h3 class="mb-2 text-lg font-semibold">AC & Heating Repair</h3>
          <p class="text-sm leading-relaxed text-gray-600">
            Ensuring your car's climate control system works perfectly year-round.
          </p>
        </div>
        <div class="rounded-lg border p-6 shadow-sm">
          <h3 class="mb-2 text-lg font-semibold">Pre-Purchase Inspections</h3>
          <p class="text-sm leading-relaxed text-gray-600">
            Thorough inspections to give you peace of mind before buying a used vehicle.
          </p>
        </div>
      </div>
    </section>
  `,
})
export class Landing {}
