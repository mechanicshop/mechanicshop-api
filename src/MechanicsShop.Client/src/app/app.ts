import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
  host: {
    class: 'block container',
  },
})
export class App {
  protected readonly title = signal('MechanicsShop.Client');
}
