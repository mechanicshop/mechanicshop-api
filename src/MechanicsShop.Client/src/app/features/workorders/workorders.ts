import { Component } from '@angular/core';

import { PageHeader } from '@shared/components/page-header';

@Component({
  selector: 'app-work-orders',
  standalone: true,
  imports: [PageHeader],
  template: `
    <div>
      <main class="container page-layout">
        <app-page-header>
          <h1>Work Orders</h1>
          <p>Work Orders feature coming soon.</p>
        </app-page-header>
      </main>
    </div>
  `,
})
export class WorkOrders {}
