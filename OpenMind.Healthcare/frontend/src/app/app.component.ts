import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from './services/api.service';

@Component({
  selector: 'app-root',
  standalone: false,
  template: `
    <app-navbar *ngIf="showNavbar"></app-navbar>
    <main class="main-content" [class.full-height]="!showNavbar">
      <router-outlet></router-outlet>
    </main>
  `,
  styles: [`
    .main-content {
      padding: 20px;
      min-height: calc(100vh - 80px);
      transition: min-height 0.3s ease;
    }
    
    .main-content.full-height {
      min-height: 100vh;
      padding: 0;
    }
  `]
})
export class AppComponent implements OnInit {
  title = 'Quit Smoking Tracker';
  showNavbar = false;

  constructor(
    private router: Router,
    private apiService: ApiService
  ) {}

  ngOnInit(): void {
    // Watch for route changes and auth state to show/hide navbar
    this.router.events.subscribe(() => {
      this.updateNavbarVisibility();
    });

    this.apiService.currentUser$.subscribe(() => {
      this.updateNavbarVisibility();
    });

    this.updateNavbarVisibility();
  }

  private updateNavbarVisibility(): void {
    const currentRoute = this.router.url;
    const isAuthRoute = currentRoute === '/login' || currentRoute === '/register';
    this.showNavbar = this.apiService.isLoggedIn && !isAuthRoute;
  }
}
