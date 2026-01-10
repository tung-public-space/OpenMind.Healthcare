import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  template: `
    <nav class="navbar">
      <div class="navbar-brand" (click)="navigate('/dashboard')">
        <span class="logo">🚭</span>
        <span class="brand-text">Smoke Free Journey</span>
      </div>
      <ul class="navbar-menu">
        <li (click)="navigate('/dashboard')" [class.active]="isActive('/dashboard')">
          <span class="icon">📊</span>
          Dashboard
        </li>
        <li (click)="navigate('/health')" [class.active]="isActive('/health')">
          <span class="icon">❤️</span>
          Health
        </li>
        <li (click)="navigate('/achievements')" [class.active]="isActive('/achievements')">
          <span class="icon">🏆</span>
          Achievements
        </li>
        <li (click)="navigate('/motivation')" [class.active]="isActive('/motivation')">
          <span class="icon">💪</span>
          Motivation
        </li>
        <li (click)="navigate('/craving-help')" [class.active]="isActive('/craving-help')" class="craving-btn">
          <span class="icon">🆘</span>
          Craving Help
        </li>
      </ul>
    </nav>
  `,
  styles: [`
    .navbar {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 15px 30px;
      background: rgba(255, 255, 255, 0.05);
      backdrop-filter: blur(10px);
      border-bottom: 1px solid rgba(255, 255, 255, 0.1);
    }
    
    .navbar-brand {
      display: flex;
      align-items: center;
      gap: 12px;
      cursor: pointer;
      transition: transform 0.3s ease;
      
      &:hover {
        transform: scale(1.05);
      }
    }
    
    .logo {
      font-size: 32px;
    }
    
    .brand-text {
      font-size: 20px;
      font-weight: 700;
      background: linear-gradient(135deg, #10b981, #34d399);
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
    }
    
    .navbar-menu {
      display: flex;
      list-style: none;
      gap: 10px;
      
      li {
        display: flex;
        align-items: center;
        gap: 8px;
        padding: 10px 16px;
        border-radius: 10px;
        cursor: pointer;
        transition: all 0.3s ease;
        font-weight: 500;
        
        &:hover {
          background: rgba(255, 255, 255, 0.1);
        }
        
        &.active {
          background: rgba(16, 185, 129, 0.2);
          color: #10b981;
        }
        
        &.craving-btn {
          background: linear-gradient(135deg, #ef4444, #dc2626);
          color: white;
          
          &:hover {
            transform: scale(1.05);
            box-shadow: 0 5px 20px rgba(239, 68, 68, 0.4);
          }
        }
      }
    }
    
    .icon {
      font-size: 18px;
    }
    
    @media (max-width: 768px) {
      .navbar {
        flex-direction: column;
        gap: 15px;
      }
      
      .navbar-menu {
        flex-wrap: wrap;
        justify-content: center;
      }
    }
  `]
})
export class NavbarComponent {
  constructor(private router: Router) {}

  navigate(path: string): void {
    this.router.navigate([path]);
  }

  isActive(path: string): boolean {
    return this.router.url === path;
  }
}
