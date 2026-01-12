import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { User } from '../../models/models';

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
        <li (click)="navigate('/calendar')" [class.active]="isActive('/calendar')">
          <span class="icon">📅</span>
          Calendar
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
      <div class="user-menu">
        <span class="username">{{ getDisplayName() }}</span>
        <button class="logout-btn" (click)="logout()">Logout</button>
      </div>
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
    
    .user-menu {
      display: flex;
      align-items: center;
      gap: 15px;
    }
    
    .username {
      font-weight: 500;
      color: #10b981;
    }
    
    .logout-btn {
      background: rgba(239, 68, 68, 0.1);
      color: #ef4444;
      border: 1px solid rgba(239, 68, 68, 0.3);
      padding: 8px 16px;
      border-radius: 6px;
      cursor: pointer;
      font-weight: 500;
      transition: all 0.3s ease;
      
      &:hover {
        background: #ef4444;
        color: white;
      }
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
      
      .user-menu {
        order: -1;
      }
    }
  `]
})
export class NavbarComponent implements OnInit {
  currentUser: User | null = null;

  constructor(
    private router: Router,
    private apiService: ApiService
  ) {}

  ngOnInit(): void {
    this.apiService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  navigate(path: string): void {
    this.router.navigate([path]);
  }

  logout(): void {
    this.apiService.logout();
    this.router.navigate(['/login']);
  }

  getDisplayName(): string {
    if (!this.currentUser) return 'Guest';
    
    const firstName = this.currentUser.firstName || '';
    const lastName = this.currentUser.lastName || '';
    
    if (firstName && lastName) {
      return `${firstName} ${lastName}`;
    } else if (firstName) {
      return firstName;
    } else if (lastName) {
      return lastName;
    } else {
      return this.currentUser.email || 'Guest';
    }
  }

  isActive(path: string): boolean {
    return this.router.url === path;
  }
}
