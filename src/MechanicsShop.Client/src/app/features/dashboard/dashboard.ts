import { CurrencyPipe, PercentPipe, DatePipe, DecimalPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatButton } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatProgressSpinner } from '@angular/material/progress-spinner';

import { todayWorkOrderStats } from '@shared/models/dashboard/dashboard.model';
import { DashboardService } from '@shared/services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  host: {
    class: 'w-full block',
  },
  imports: [
    CurrencyPipe,
    PercentPipe,
    DatePipe,
    DecimalPipe,
    MatButton,
    MatCardModule,
    MatIcon,
    MatProgressSpinner,
  ],
  template: `
    <div class="min-h-screen bg-slate-50 text-slate-900 font-sans pb-12 container">
      <!-- Navbar / Header -->
      <header class="border-b border-slate-200 bg-white sticky top-0 z-10 px-8 py-4 mb-8">
        <div
          class="w-full flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4"
        >
          <div>
            <h1 class="text-2xl font-bold tracking-tight text-slate-900 flex items-center gap-2">
              <mat-icon color="primary">dashboard</mat-icon>
              Dashboard
            </h1>
          </div>
          <button
            class="rounded-lg text-slate-700 border-slate-300 hover:bg-slate-50 flex items-center gap-2"
            (click)="statsResource.reload()"
            mat-stroked-button
          >
            <mat-icon class="scale-90">refresh</mat-icon>
            Reload Stats
          </button>
        </div>
      </header>

      <!-- Main Content Container -->
      <main class="w-full px-8">
        <!-- Loading State -->
        @if (statsResource.isLoading()) {
          <div class="flex flex-col items-center justify-center py-32 gap-4">
            <mat-progress-spinner
              mode="indeterminate"
              diameter="40"
              color="primary"
            ></mat-progress-spinner>
            <p class="text-slate-500 text-sm">Loading statistics...</p>
          </div>
        }

        <!-- Error State -->
        @else if (statsResource.error()) {
          <div
            class="max-w-xl mx-auto bg-white border border-rose-200 p-8 rounded-xl text-center flex flex-col items-center gap-6 mt-12 shadow-sm"
          >
            <div class="p-3 bg-rose-50 text-rose-500 rounded-full">
              <mat-icon class="scale-125">error_outline</mat-icon>
            </div>
            <div>
              <h3 class="text-lg font-semibold text-rose-800">Failed to load statistics</h3>
              <p class="text-slate-600 text-sm mt-2">
                An error occurred while connecting to the server. Please check your network and try
                again.
              </p>
            </div>
            <button
              class="text-rose-600 border-rose-200 hover:bg-rose-50"
              (click)="statsResource.reload()"
              mat-stroked-button
            >
              Retry Request
            </button>
          </div>
        }

        <!-- Loaded Content State -->
        @else if (statsResource.value(); as stats) {
          <div class="flex flex-col gap-6 animate-fade-in">
            <!-- Date Display Header -->
            <div class="text-slate-500 text-sm mb-2 font-medium">
              Statistics for {{ stats.date | date: 'longDate' }}
            </div>

            <!-- Row 1: Status Breakdown (5 columns) -->
            <div class="grid grid-cols-2 md:grid-cols-5 gap-4">
              <!-- Total Orders -->
              <mat-card
                class="bg-white border-slate-200 text-slate-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-slate-500 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-blue-500 scale-90">assignment</mat-icon>
                    <span>Total Orders</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-slate-900">{{ stats.total }}</div>
                </mat-card-content>
              </mat-card>

              <!-- Scheduled -->
              <mat-card
                class="bg-amber-50 border-amber-200 text-amber-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-amber-700 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-amber-600 scale-90">hourglass_empty</mat-icon>
                    <span>Scheduled</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-amber-800">{{ stats.scheduled }}</div>
                </mat-card-content>
              </mat-card>

              <!-- In Progress -->
              <mat-card
                class="bg-cyan-50 border-cyan-200 text-cyan-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-cyan-700 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-cyan-600 scale-90">sync</mat-icon>
                    <span>In Progress</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-cyan-800">{{ stats.inProgress }}</div>
                </mat-card-content>
              </mat-card>

              <!-- Completed -->
              <mat-card
                class="bg-emerald-50 border-emerald-200 text-emerald-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-emerald-700 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-emerald-600 scale-90">check_circle</mat-icon>
                    <span>Completed</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-emerald-800">{{ stats.completed }}</div>
                </mat-card-content>
              </mat-card>

              <!-- Cancelled -->
              <mat-card
                class="bg-rose-50 border-rose-200 text-rose-900 shadow-sm col-span-2 md:col-span-1"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-rose-700 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-rose-600 scale-90">cancel</mat-icon>
                    <span>Cancelled</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-rose-800">{{ stats.cancelled }}</div>
                </mat-card-content>
              </mat-card>
            </div>

            <!-- Row 2: Financial Overview (4 columns) -->
            <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
              <!-- Total Revenue -->
              <mat-card
                class="bg-white border-slate-200 text-slate-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-slate-500 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-purple-500 scale-90">attach_money</mat-icon>
                    <span>Total Revenue</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-slate-900">
                    {{ stats.totalRevenue | currency }}
                  </div>
                </mat-card-content>
              </mat-card>

              <!-- Total Parts Cost -->
              <mat-card
                class="bg-white border-slate-200 text-slate-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-slate-500 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-orange-500 scale-90">build</mat-icon>
                    <span>Total Parts Cost</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-slate-900">
                    {{ stats.totalPartsCost | currency }}
                  </div>
                </mat-card-content>
              </mat-card>

              <!-- Total Labor Cost -->
              <mat-card
                class="bg-white border-slate-200 text-slate-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-slate-500 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-orange-500 scale-90">person</mat-icon>
                    <span>Total Labor Cost</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-slate-900">
                    {{ stats.totalLaborCost | currency }}
                  </div>
                </mat-card-content>
              </mat-card>

              <!-- Net Profit -->
              <mat-card
                class="bg-white border-slate-200 text-slate-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-slate-500 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-teal-500 scale-90">account_balance_wallet</mat-icon>
                    <span>Net Profit</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-slate-900">
                    {{ stats.netProfit | currency }}
                  </div>
                </mat-card-content>
              </mat-card>
            </div>

            <!-- Row 3: Unique Counts & Margin (3 columns) -->
            <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
              <!-- Unique Vehicles -->
              <mat-card
                class="bg-white border-slate-200 text-slate-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-slate-500 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-purple-500 scale-90">directions_car</mat-icon>
                    <span>Unique Vehicles</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-slate-900">
                    {{ stats.uniqueVehicles }}
                  </div>
                </mat-card-content>
              </mat-card>

              <!-- Unique Customers -->
              <mat-card
                class="bg-white border-slate-200 text-slate-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-slate-500 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-pink-500 scale-90">people</mat-icon>
                    <span>Unique Customers</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-slate-900">
                    {{ stats.uniqueCustomers }}
                  </div>
                </mat-card-content>
              </mat-card>

              <!-- Profit Margin -->
              <mat-card
                class="bg-white border-slate-200 text-slate-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-slate-500 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-blue-500 scale-90">trending_up</mat-icon>
                    <span>Profit Margin</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-slate-900">
                    {{ stats.profitMargin / 100 | percent: '1.2-2' }}
                  </div>
                </mat-card-content>
              </mat-card>
            </div>

            <!-- Row 4: Performance Ratios (3 columns) -->
            <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
              <!-- Completion Rate -->
              <mat-card
                class="bg-white border-slate-200 text-slate-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-slate-500 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-cyan-550 scale-90">assignment_turned_in</mat-icon>
                    <span>Completion Rate</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-slate-900">
                    {{ stats.completionRate / 100 | percent: '1.2-2' }}
                  </div>
                </mat-card-content>
              </mat-card>

              <!-- Avg. Revenue/Order -->
              <mat-card
                class="bg-white border-slate-200 text-slate-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-slate-500 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-slate-500 scale-90">analytics</mat-icon>
                    <span>Avg. Revenue/Order</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-slate-900">
                    {{ stats.averageRevenuePerOrder | currency }}
                  </div>
                </mat-card-content>
              </mat-card>

              <!-- Orders Per Vehicle -->
              <mat-card
                class="bg-white border-slate-200 text-slate-900 shadow-sm"
                appearance="outlined"
              >
                <mat-card-content class="p-4 flex flex-col justify-between h-full">
                  <div
                    class="flex items-center gap-2 text-slate-500 text-xs font-semibold tracking-wide uppercase"
                  >
                    <mat-icon class="text-slate-500 scale-90">speed</mat-icon>
                    <span>Orders Per Vehicle</span>
                  </div>
                  <div class="text-3xl font-bold mt-4 text-slate-900">
                    {{ stats.ordersPerVehicle | number: '1.2-2' }}
                  </div>
                </mat-card-content>
              </mat-card>
            </div>
          </div>
        }
      </main>
    </div>
  `,
  styles: [
    `
      .animate-fade-in {
        animation: fadeIn 0.3s ease-out forwards;
      }
      @keyframes fadeIn {
        from {
          opacity: 0;
        }
        to {
          opacity: 1;
        }
      }
    `,
  ],
})
export class Dashboard {
  private readonly dashboardService = inject(DashboardService);

  readonly statsResource = rxResource<todayWorkOrderStats, undefined>({
    stream: () => this.dashboardService.getWorkOrderStats(),
  });
}
