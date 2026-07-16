import { Component } from '@angular/core';

import { PageHeader } from '@shared/components/page-header';

@Component({
  selector: 'app-schedules',
  standalone: true,
  imports: [PageHeader],
  template: `
    <div>
      <main class="container page-layout">
        <app-page-header>
          <h1>Schedules</h1>
          <p>Schedules feature coming soon.</p>
        </app-page-header>
      </main>
    </div>
  `,
})
export class Schedules {}
