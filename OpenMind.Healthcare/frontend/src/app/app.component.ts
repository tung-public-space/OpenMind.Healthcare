import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <app-navbar></app-navbar>
    <main class="main-content">
      <router-outlet></router-outlet>
    </main>
  `,
  styles: [`
    .main-content {
      padding: 20px;
      min-height: calc(100vh - 80px);
    }
  `]
})
export class AppComponent {
  title = 'Quit Smoking Tracker';
}
