import { Component } from '@angular/core';

import { PageHeader } from '@shared/components/page-header';

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [PageHeader],
  template: `
    <div>
      <main class="container page-layout">
        <app-page-header>
          <h1>Customers</h1>
          <p>Customers feature coming soon.</p>
        </app-page-header>
      </main>
    </div>
  `,
})
export class Customers {}
