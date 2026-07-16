import { Component } from '@angular/core';

import { PageHeader } from '@shared/components/page-header';

@Component({
  selector: 'app-repair-tasks',
  standalone: true,
  imports: [PageHeader],
  template: `
    <div>
      <main class="container page-layout">
        <app-page-header>
          <h1>Repair Tasks</h1>
          <p>Repair Tasks feature coming soon.</p>
        </app-page-header>
      </main>
    </div>
  `,
})
export class RepairTasks {}
