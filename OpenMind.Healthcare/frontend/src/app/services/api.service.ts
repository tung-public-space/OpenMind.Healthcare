import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
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

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'http://localhost:3003/api';
  private userApiUrl = 'http://localhost:3004/api';
  
  private progressSubject = new BehaviorSubject<UserProgress | null>(null);
  public progress$ = this.progressSubject.asObservable();

  private statsSubject = new BehaviorSubject<ProgressStats | null>(null);
  public stats$ = this.statsSubject.asObservable();

  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    // Load current user from localStorage
    const savedUser = localStorage.getItem('currentUser');
    if (savedUser) {
      this.currentUserSubject.next(JSON.parse(savedUser));
      this.loadProgress();
    }
  }

  // Authentication methods
  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.userApiUrl}/auth/login`, credentials).pipe(
      tap(response => {
        if (response.token) {
          localStorage.setItem('auth_token', response.token);
          const user: User = {
            id: response.id,
            email: response.email,
            username: response.username,
            firstName: response.firstName,
            lastName: response.lastName,
            createdAt: '',
            lastLoginAt: ''
          };
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
          this.loadProgress();
        }
      })
    );
  }

  register(userData: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.userApiUrl}/auth/register`, userData).pipe(
      tap(response => {
        if (response.token) {
          localStorage.setItem('auth_token', response.token);
          const user: User = {
            id: response.id,
            email: response.email,
            username: response.username,
            firstName: response.firstName,
            lastName: response.lastName,
            createdAt: '',
            lastLoginAt: ''
          };
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.progressSubject.next(null);
    this.statsSubject.next(null);
  }

  get isLoggedIn(): boolean {
    return !!localStorage.getItem('auth_token');
  }

  get currentUser(): User | null {
    return this.currentUserSubject.value;
  }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('auth_token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  // Progress endpoints
  getProgress(): Observable<UserProgress> {
    return this.http.get<UserProgress>(`${this.baseUrl}/progress`, { headers: this.getAuthHeaders() }).pipe(
      tap(progress => this.progressSubject.next(progress))
    );
  }

  saveProgress(progress: Partial<UserProgress>): Observable<UserProgress> {
    return this.http.post<UserProgress>(`${this.baseUrl}/progress`, progress, { headers: this.getAuthHeaders() }).pipe(
      tap(saved => this.progressSubject.next(saved))
    );
  }

  getStats(): Observable<ProgressStats> {
    return this.http.get<ProgressStats>(`${this.baseUrl}/progress/stats`, { headers: this.getAuthHeaders() }).pipe(
      tap(stats => this.statsSubject.next(stats))
    );
  }

  getHealthMilestones(): Observable<HealthMilestone[]> {
    return this.http.get<HealthMilestone[]>(`${this.baseUrl}/progress/health-milestones`, { headers: this.getAuthHeaders() });
  }

  // Achievement endpoints
  getAchievements(): Observable<Achievement[]> {
    return this.http.get<Achievement[]>(`${this.baseUrl}/achievements`, { headers: this.getAuthHeaders() });
  }

  getUnlockedAchievements(): Observable<Achievement[]> {
    return this.http.get<Achievement[]>(`${this.baseUrl}/achievements/unlocked`, { headers: this.getAuthHeaders() });
  }

  checkNewAchievement(): Observable<Achievement | null> {
    return this.http.get<Achievement | null>(`${this.baseUrl}/achievements/check-new`, { headers: this.getAuthHeaders() });
  }

  // Motivation endpoints
  getRandomQuote(): Observable<MotivationalQuote> {
    return this.http.get<MotivationalQuote>(`${this.baseUrl}/motivation/quote`, { headers: this.getAuthHeaders() });
  }

  getCravingTips(): Observable<CravingTip[]> {
    return this.http.get<CravingTip[]>(`${this.baseUrl}/motivation/craving-tips`, { headers: this.getAuthHeaders() });
  }

  getDailyEncouragement(): Observable<DailyEncouragement> {
    return this.http.get<DailyEncouragement>(`${this.baseUrl}/motivation/daily`, { headers: this.getAuthHeaders() });
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
