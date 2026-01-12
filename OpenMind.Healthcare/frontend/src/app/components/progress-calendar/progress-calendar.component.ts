import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { UserProgress, Achievement, ProgressStats } from '../../models/models';

interface CalendarDay {
  date: Date;
  dayNumber: number;
  isCurrentMonth: boolean;
  isToday: boolean;
  isQuitDay: boolean;
  daysSinceQuit: number;
  status: 'before-quit' | 'smoke-free' | 'future';
  achievements: Achievement[];
  moneySaved: number;
  cigarettesAvoided: number;
}

interface CalendarWeek {
  days: CalendarDay[];
}

@Component({
  selector: 'app-progress-calendar',
  standalone: false,
  template: `
    <div class="calendar-container fade-in">
      <div class="calendar-header">
        <h1>📅 Progress Calendar</h1>
        <p class="subtitle">Track your smoke-free journey day by day</p>
      </div>

      <div class="calendar-card" *ngIf="progress">
        <!-- Month Navigation -->
        <div class="month-navigation">
          <button class="nav-btn" (click)="previousMonth()">
            <span>←</span>
          </button>
          <h2 class="current-month">{{ currentDate | date:'MMMM yyyy' }}</h2>
          <button class="nav-btn" (click)="nextMonth()">
            <span>→</span>
          </button>
        </div>

        <!-- Weekday Headers -->
        <div class="weekday-headers">
          <div class="weekday" *ngFor="let day of weekDays">{{ day }}</div>
        </div>

        <!-- Calendar Grid -->
        <div class="calendar-grid">
          <div 
            *ngFor="let week of calendarWeeks" 
            class="calendar-week">
            <div 
              *ngFor="let day of week.days"
              class="calendar-day"
              [class.other-month]="!day.isCurrentMonth"
              [class.today]="day.isToday"
              [class.quit-day]="day.isQuitDay"
              [class.smoke-free]="day.status === 'smoke-free'"
              [class.before-quit]="day.status === 'before-quit'"
              [class.future]="day.status === 'future'"
              (click)="selectDay(day)">
              <span class="day-number">{{ day.dayNumber }}</span>
              <div class="day-indicators" *ngIf="day.isCurrentMonth && day.status === 'smoke-free'">
                <span class="streak-badge" *ngIf="day.daysSinceQuit > 0">
                  Day {{ day.daysSinceQuit }}
                </span>
                <span class="achievement-indicator" *ngIf="day.achievements.length > 0">
                  🏆
                </span>
              </div>
              <div class="quit-badge" *ngIf="day.isQuitDay">
                🚭
              </div>
            </div>
          </div>
        </div>

        <!-- Legend -->
        <div class="calendar-legend">
          <div class="legend-item">
            <span class="legend-color quit-day"></span>
            <span>Quit Day</span>
          </div>
          <div class="legend-item">
            <span class="legend-color smoke-free"></span>
            <span>Smoke-Free</span>
          </div>
          <div class="legend-item">
            <span class="legend-icon">🏆</span>
            <span>Achievement</span>
          </div>
          <div class="legend-item">
            <span class="legend-color today"></span>
            <span>Today</span>
          </div>
        </div>
      </div>

      <!-- Selected Day Details -->
      <div class="day-details" *ngIf="selectedDay && selectedDay.status === 'smoke-free'">
        <div class="details-card">
          <h3>{{ selectedDay.date | date:'EEEE, MMMM d, yyyy' }}</h3>
          
          <div class="details-grid">
            <div class="detail-item">
              <span class="detail-icon">📆</span>
              <span class="detail-value">Day {{ selectedDay.daysSinceQuit }}</span>
              <span class="detail-label">of your journey</span>
            </div>
            <div class="detail-item">
              <span class="detail-icon">🚬</span>
              <span class="detail-value">{{ selectedDay.cigarettesAvoided }}</span>
              <span class="detail-label">cigarettes avoided</span>
            </div>
            <div class="detail-item">
              <span class="detail-icon">💰</span>
              <span class="detail-value">\${{ selectedDay.moneySaved | number:'1.2-2' }}</span>
              <span class="detail-label">money saved</span>
            </div>
          </div>

          <div class="achievements-section" *ngIf="selectedDay.achievements.length > 0">
            <h4>🏆 Achievements Unlocked</h4>
            <div class="achievement-list">
              <div class="achievement-item" *ngFor="let achievement of selectedDay.achievements">
                <span class="achievement-icon">{{ achievement.icon }}</span>
                <div class="achievement-info">
                  <span class="achievement-name">{{ achievement.name }}</span>
                  <span class="achievement-desc">{{ achievement.description }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Stats Summary -->
      <div class="stats-summary" *ngIf="stats">
        <div class="summary-card">
          <h3>📊 Monthly Summary</h3>
          <div class="summary-grid">
            <div class="summary-item">
              <span class="summary-value">{{ getSmokeFreeThisMonth() }}</span>
              <span class="summary-label">Smoke-free days this month</span>
            </div>
            <div class="summary-item">
              <span class="summary-value">{{ stats.daysSmokeFree }}</span>
              <span class="summary-label">Total days smoke-free</span>
            </div>
            <div class="summary-item">
              <span class="summary-value">\${{ stats.moneySaved | number:'1.2-2' }}</span>
              <span class="summary-label">Total money saved</span>
            </div>
            <div class="summary-item">
              <span class="summary-value">{{ unlockedAchievements.length }}</span>
              <span class="summary-label">Achievements earned</span>
            </div>
          </div>
        </div>
      </div>

      <!-- No Progress Message -->
      <div class="no-progress" *ngIf="!progress">
        <div class="empty-state">
          <span class="empty-icon">📅</span>
          <h2>Start Your Journey</h2>
          <p>Set up your quit date to start tracking your progress on the calendar.</p>
          <button class="btn btn-primary" routerLink="/setup">Get Started</button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .calendar-container {
      padding: 20px;
      max-width: 900px;
      margin: 0 auto;
    }

    .calendar-header {
      text-align: center;
      margin-bottom: 30px;

      h1 {
        font-size: 2.5rem;
        margin-bottom: 10px;
        background: linear-gradient(135deg, #10b981, #34d399);
        -webkit-background-clip: text;
        -webkit-text-fill-color: transparent;
      }

      .subtitle {
        color: rgba(255, 255, 255, 0.7);
        font-size: 1.1rem;
      }
    }

    .calendar-card {
      background: rgba(255, 255, 255, 0.05);
      border-radius: 20px;
      padding: 25px;
      border: 1px solid rgba(255, 255, 255, 0.1);
    }

    .month-navigation {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 25px;

      .nav-btn {
        background: rgba(255, 255, 255, 0.1);
        border: none;
        width: 40px;
        height: 40px;
        border-radius: 50%;
        cursor: pointer;
        transition: all 0.3s ease;
        color: white;
        font-size: 1.2rem;

        &:hover {
          background: rgba(16, 185, 129, 0.3);
          transform: scale(1.1);
        }
      }

      .current-month {
        font-size: 1.5rem;
        font-weight: 600;
        color: white;
      }
    }

    .weekday-headers {
      display: grid;
      grid-template-columns: repeat(7, 1fr);
      gap: 5px;
      margin-bottom: 10px;

      .weekday {
        text-align: center;
        padding: 10px;
        font-weight: 600;
        color: rgba(255, 255, 255, 0.6);
        font-size: 0.85rem;
      }
    }

    .calendar-grid {
      display: flex;
      flex-direction: column;
      gap: 5px;
    }

    .calendar-week {
      display: grid;
      grid-template-columns: repeat(7, 1fr);
      gap: 5px;
    }

    .calendar-day {
      aspect-ratio: 1;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      border-radius: 12px;
      cursor: pointer;
      transition: all 0.3s ease;
      position: relative;
      background: rgba(255, 255, 255, 0.03);
      min-height: 70px;

      .day-number {
        font-size: 1rem;
        font-weight: 500;
        color: white;
      }

      &.other-month {
        opacity: 0.3;
        
        .day-number {
          color: rgba(255, 255, 255, 0.4);
        }
      }

      &.today {
        border: 2px solid #10b981;
        box-shadow: 0 0 15px rgba(16, 185, 129, 0.3);
      }

      &.quit-day {
        background: linear-gradient(135deg, #10b981, #059669);
        
        .day-number {
          font-weight: 700;
        }
      }

      &.smoke-free {
        background: rgba(16, 185, 129, 0.2);

        &:hover {
          background: rgba(16, 185, 129, 0.4);
          transform: scale(1.05);
        }
      }

      &.before-quit {
        background: rgba(255, 255, 255, 0.02);
      }

      &.future {
        background: rgba(255, 255, 255, 0.02);
        opacity: 0.5;
      }

      .day-indicators {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 2px;
        margin-top: 2px;
      }

      .streak-badge {
        font-size: 0.6rem;
        color: #10b981;
        font-weight: 600;
      }

      .achievement-indicator {
        font-size: 0.8rem;
      }

      .quit-badge {
        position: absolute;
        top: 5px;
        right: 5px;
        font-size: 0.8rem;
      }
    }

    .calendar-legend {
      display: flex;
      justify-content: center;
      gap: 25px;
      margin-top: 25px;
      padding-top: 20px;
      border-top: 1px solid rgba(255, 255, 255, 0.1);

      .legend-item {
        display: flex;
        align-items: center;
        gap: 8px;
        font-size: 0.85rem;
        color: rgba(255, 255, 255, 0.7);
      }

      .legend-color {
        width: 16px;
        height: 16px;
        border-radius: 4px;

        &.quit-day {
          background: linear-gradient(135deg, #10b981, #059669);
        }

        &.smoke-free {
          background: rgba(16, 185, 129, 0.4);
        }

        &.today {
          border: 2px solid #10b981;
          background: transparent;
        }
      }

      .legend-icon {
        font-size: 1rem;
      }
    }

    .day-details {
      margin-top: 25px;

      .details-card {
        background: rgba(255, 255, 255, 0.05);
        border-radius: 16px;
        padding: 25px;
        border: 1px solid rgba(255, 255, 255, 0.1);

        h3 {
          text-align: center;
          margin-bottom: 20px;
          color: #10b981;
        }
      }

      .details-grid {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 20px;
        margin-bottom: 20px;
      }

      .detail-item {
        display: flex;
        flex-direction: column;
        align-items: center;
        text-align: center;
        padding: 15px;
        background: rgba(255, 255, 255, 0.03);
        border-radius: 12px;

        .detail-icon {
          font-size: 1.5rem;
          margin-bottom: 8px;
        }

        .detail-value {
          font-size: 1.3rem;
          font-weight: 700;
          color: white;
        }

        .detail-label {
          font-size: 0.8rem;
          color: rgba(255, 255, 255, 0.6);
          margin-top: 4px;
        }
      }

      .achievements-section {
        padding-top: 20px;
        border-top: 1px solid rgba(255, 255, 255, 0.1);

        h4 {
          margin-bottom: 15px;
          color: #fbbf24;
        }
      }

      .achievement-list {
        display: flex;
        flex-direction: column;
        gap: 12px;
      }

      .achievement-item {
        display: flex;
        align-items: center;
        gap: 15px;
        padding: 12px;
        background: rgba(251, 191, 36, 0.1);
        border-radius: 10px;
        border: 1px solid rgba(251, 191, 36, 0.2);

        .achievement-icon {
          font-size: 2rem;
        }

        .achievement-info {
          display: flex;
          flex-direction: column;

          .achievement-name {
            font-weight: 600;
            color: white;
          }

          .achievement-desc {
            font-size: 0.85rem;
            color: rgba(255, 255, 255, 0.6);
          }
        }
      }
    }

    .stats-summary {
      margin-top: 25px;

      .summary-card {
        background: rgba(255, 255, 255, 0.05);
        border-radius: 16px;
        padding: 25px;
        border: 1px solid rgba(255, 255, 255, 0.1);

        h3 {
          text-align: center;
          margin-bottom: 20px;
          color: white;
        }
      }

      .summary-grid {
        display: grid;
        grid-template-columns: repeat(4, 1fr);
        gap: 15px;
      }

      .summary-item {
        display: flex;
        flex-direction: column;
        align-items: center;
        text-align: center;
        padding: 15px;
        background: rgba(255, 255, 255, 0.03);
        border-radius: 12px;

        .summary-value {
          font-size: 1.5rem;
          font-weight: 700;
          color: #10b981;
        }

        .summary-label {
          font-size: 0.8rem;
          color: rgba(255, 255, 255, 0.6);
          margin-top: 5px;
        }
      }
    }

    .no-progress {
      .empty-state {
        text-align: center;
        padding: 60px 20px;
        background: rgba(255, 255, 255, 0.05);
        border-radius: 20px;
        border: 1px solid rgba(255, 255, 255, 0.1);

        .empty-icon {
          font-size: 4rem;
          display: block;
          margin-bottom: 20px;
        }

        h2 {
          margin-bottom: 10px;
          color: white;
        }

        p {
          color: rgba(255, 255, 255, 0.6);
          margin-bottom: 25px;
        }
      }
    }

    .btn {
      padding: 12px 30px;
      border: none;
      border-radius: 25px;
      font-size: 1rem;
      font-weight: 600;
      cursor: pointer;
      transition: all 0.3s ease;

      &.btn-primary {
        background: linear-gradient(135deg, #10b981, #059669);
        color: white;

        &:hover {
          transform: translateY(-2px);
          box-shadow: 0 10px 30px rgba(16, 185, 129, 0.4);
        }
      }
    }

    .fade-in {
      animation: fadeIn 0.5s ease-out;
    }

    @keyframes fadeIn {
      from {
        opacity: 0;
        transform: translateY(20px);
      }
      to {
        opacity: 1;
        transform: translateY(0);
      }
    }

    @media (max-width: 768px) {
      .calendar-container {
        padding: 15px;
      }

      .calendar-header h1 {
        font-size: 1.8rem;
      }

      .calendar-day {
        min-height: 50px;

        .day-number {
          font-size: 0.85rem;
        }

        .streak-badge {
          display: none;
        }
      }

      .calendar-legend {
        flex-wrap: wrap;
        gap: 15px;
      }

      .day-details .details-grid {
        grid-template-columns: 1fr;
      }

      .stats-summary .summary-grid {
        grid-template-columns: repeat(2, 1fr);
      }
    }
  `]
})
export class ProgressCalendarComponent implements OnInit {
  currentDate = new Date();
  progress: UserProgress | null = null;
  stats: ProgressStats | null = null;
  achievements: Achievement[] = [];
  unlockedAchievements: Achievement[] = [];
  calendarWeeks: CalendarWeek[] = [];
  selectedDay: CalendarDay | null = null;
  weekDays = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.apiService.getProgress().subscribe({
      next: (progress) => {
        this.progress = progress;
        this.loadStats();
        this.loadAchievements();
      },
      error: () => {
        this.progress = null;
      }
    });
  }

  loadStats(): void {
    this.apiService.getStats().subscribe({
      next: (stats) => {
        this.stats = stats;
        this.generateCalendar();
      }
    });
  }

  loadAchievements(): void {
    this.apiService.getAchievements().subscribe({
      next: (achievements) => {
        this.achievements = achievements;
        this.unlockedAchievements = achievements.filter(a => a.isUnlocked);
        this.generateCalendar();
      }
    });
  }

  generateCalendar(): void {
    if (!this.progress) return;

    const year = this.currentDate.getFullYear();
    const month = this.currentDate.getMonth();
    const quitDate = new Date(this.progress.quitDate);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const firstDayOfMonth = new Date(year, month, 1);
    const lastDayOfMonth = new Date(year, month + 1, 0);
    const startingDayOfWeek = firstDayOfMonth.getDay();
    const totalDaysInMonth = lastDayOfMonth.getDate();

    const weeks: CalendarWeek[] = [];
    let currentWeek: CalendarDay[] = [];

    // Fill in days from previous month
    const prevMonth = new Date(year, month, 0);
    const prevMonthDays = prevMonth.getDate();
    for (let i = startingDayOfWeek - 1; i >= 0; i--) {
      const dayNum = prevMonthDays - i;
      const date = new Date(year, month - 1, dayNum);
      currentWeek.push(this.createCalendarDay(date, dayNum, false, quitDate, today));
    }

    // Fill in current month days
    for (let day = 1; day <= totalDaysInMonth; day++) {
      const date = new Date(year, month, day);
      currentWeek.push(this.createCalendarDay(date, day, true, quitDate, today));

      if (currentWeek.length === 7) {
        weeks.push({ days: currentWeek });
        currentWeek = [];
      }
    }

    // Fill in days from next month
    if (currentWeek.length > 0) {
      let nextMonthDay = 1;
      while (currentWeek.length < 7) {
        const date = new Date(year, month + 1, nextMonthDay);
        currentWeek.push(this.createCalendarDay(date, nextMonthDay, false, quitDate, today));
        nextMonthDay++;
      }
      weeks.push({ days: currentWeek });
    }

    this.calendarWeeks = weeks;
  }

  createCalendarDay(date: Date, dayNumber: number, isCurrentMonth: boolean, quitDate: Date, today: Date): CalendarDay {
    const dateOnly = new Date(date);
    dateOnly.setHours(0, 0, 0, 0);
    
    const quitDateOnly = new Date(quitDate);
    quitDateOnly.setHours(0, 0, 0, 0);

    const isQuitDay = dateOnly.getTime() === quitDateOnly.getTime();
    const isToday = dateOnly.getTime() === today.getTime();
    
    let status: 'before-quit' | 'smoke-free' | 'future';
    let daysSinceQuit = 0;

    if (dateOnly < quitDateOnly) {
      status = 'before-quit';
    } else if (dateOnly > today) {
      status = 'future';
    } else {
      status = 'smoke-free';
      daysSinceQuit = Math.floor((dateOnly.getTime() - quitDateOnly.getTime()) / (1000 * 60 * 60 * 24)) + 1;
    }

    // Find achievements for this day
    const achievementsForDay = this.unlockedAchievements.filter(a => {
      return a.requiredDays === daysSinceQuit - 1;
    });

    // Calculate cumulative savings up to this day
    const cigarettesPerDay = this.progress?.cigarettesPerDay || 0;
    const pricePerPack = this.progress?.pricePerPack || 0;
    const cigarettesPerPack = this.progress?.cigarettesPerPack || 20;
    
    const cigarettesAvoided = daysSinceQuit * cigarettesPerDay;
    const moneySaved = (cigarettesAvoided / cigarettesPerPack) * pricePerPack;

    return {
      date: dateOnly,
      dayNumber,
      isCurrentMonth,
      isToday,
      isQuitDay,
      daysSinceQuit: status === 'smoke-free' ? daysSinceQuit : 0,
      status,
      achievements: achievementsForDay,
      moneySaved: status === 'smoke-free' ? moneySaved : 0,
      cigarettesAvoided: status === 'smoke-free' ? cigarettesAvoided : 0
    };
  }

  previousMonth(): void {
    this.currentDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth() - 1, 1);
    this.generateCalendar();
    this.selectedDay = null;
  }

  nextMonth(): void {
    this.currentDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth() + 1, 1);
    this.generateCalendar();
    this.selectedDay = null;
  }

  selectDay(day: CalendarDay): void {
    if (day.isCurrentMonth && day.status === 'smoke-free') {
      this.selectedDay = day;
    }
  }

  getSmokeFreeThisMonth(): number {
    return this.calendarWeeks.reduce((total, week) => {
      return total + week.days.filter(d => d.isCurrentMonth && d.status === 'smoke-free').length;
    }, 0);
  }
}
