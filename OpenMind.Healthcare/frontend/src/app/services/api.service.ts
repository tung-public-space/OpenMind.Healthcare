import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { 
  UserProgress, 
  ProgressStats, 
  Achievement, 
  HealthMilestone, 
  MotivationalQuote, 
  CravingTip, 
  DailyEncouragement,
  LoginRequest,
  RegisterRequest,
  AuthResponse,
  User 
} from '../models/models';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = '/api';
  
  private progressSubject = new BehaviorSubject<UserProgress | null>(null);
  public progress$ = this.progressSubject.asObservable();

  private statsSubject = new BehaviorSubject<ProgressStats | null>(null);
  public stats$ = this.statsSubject.asObservable();

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {
    // Load progress if user is logged in
    if (this.authService.isLoggedIn) {
      this.loadProgress();
    }
  }

  // Expose auth service observables
  get currentUser$(): Observable<User | null> {
    return this.authService.currentUser$;
  }

  // Authentication methods - delegate to AuthService
  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.authService.login(credentials).pipe(
      tap(() => this.loadProgress())
    );
  }

  register(userData: RegisterRequest): Observable<AuthResponse> {
    return this.authService.register(userData);
  }

  logout(): void {
    this.authService.logout();
    this.progressSubject.next(null);
    this.statsSubject.next(null);
  }

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn;
  }

  get currentUser(): User | null {
    return this.authService.currentUser;
  }

  // Progress endpoints - no longer need auth headers, interceptor handles it
  getProgress(): Observable<UserProgress> {
    return this.http.get<UserProgress>(`${this.baseUrl}/progress`).pipe(
      tap(progress => this.progressSubject.next(progress))
    );
  }

  saveProgress(progress: Partial<UserProgress>): Observable<UserProgress> {
    return this.http.post<UserProgress>(`${this.baseUrl}/progress`, progress).pipe(
      tap(saved => this.progressSubject.next(saved))
    );
  }

  getStats(): Observable<ProgressStats> {
    return this.http.get<ProgressStats>(`${this.baseUrl}/progress/stats`).pipe(
      tap(stats => this.statsSubject.next(stats))
    );
  }

  getHealthMilestones(): Observable<HealthMilestone[]> {
    return this.http.get<HealthMilestone[]>(`${this.baseUrl}/progress/health-milestones`);
  }

  // Achievement endpoints
  getAchievements(): Observable<Achievement[]> {
    return this.http.get<Achievement[]>(`${this.baseUrl}/achievements`);
  }

  getUnlockedAchievements(): Observable<Achievement[]> {
    return this.http.get<Achievement[]>(`${this.baseUrl}/achievements/unlocked`);
  }

  checkNewAchievement(): Observable<Achievement | null> {
    return this.http.get<Achievement | null>(`${this.baseUrl}/achievements/check-new`);
  }

  // Motivation endpoints
  getRandomQuote(): Observable<MotivationalQuote> {
    return this.http.get<MotivationalQuote>(`${this.baseUrl}/motivation/quote`);
  }

  getCravingTips(): Observable<CravingTip[]> {
    return this.http.get<CravingTip[]>(`${this.baseUrl}/motivation/craving-tips`);
  }

  getDailyEncouragement(): Observable<DailyEncouragement> {
    return this.http.get<DailyEncouragement>(`${this.baseUrl}/motivation/daily`);
  }

  private loadProgress(): void {
    if (this.isLoggedIn) {
      this.getProgress().subscribe({
        error: () => {} // Handle case where no progress exists yet
      });
    }
  }

  refreshStats(): void {
    this.getStats().subscribe();
  }
}
