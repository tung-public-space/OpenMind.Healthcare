export interface UserProgress {
  id: number;
  quitDate: string;
  cigarettesPerDay: number;
  pricePerPack: number;
  cigarettesPerPack: number;
  currency: string;
  createdAt: string;
  updatedAt: string;
}

export interface MoneySaved {
  amount: number;
  currency: string;
  symbol: string;
}

export interface ProgressStats {
  daysSmokeFree: number;
  hoursSmokeFree: number;
  minutesSmokeFree: number;
  cigarettesNotSmoked: number;
  moneySaved: MoneySaved | number;
  lifeRegainedMinutes: number;
  lifeRegainedFormatted: string;
  progressPercentage: number;
  currentMilestone: string;
  nextMilestone: string;
  daysToNextMilestone: number;
}

export interface Achievement {
  id: number;
  name: string;
  description: string;
  icon: string;
  requiredDays: number;
  category: string;
  isUnlocked: boolean;
  unlockedAt?: string;
}

export interface HealthMilestone {
  id: number;
  title: string;
  description: string;
  timeInMinutes: number;
  timeDisplay: string;
  icon: string;
  category: string;
  isAchieved: boolean;
  progressPercentage: number;
}

export interface MotivationalQuote {
  id: number;
  quote: string;
  author: string;
  category: string;
}

export interface CravingTip {
  id: number;
  title: string;
  description: string;
  icon: string;
  category: string;
}

export interface DailyEncouragement {
  message: string;
  quote: MotivationalQuote;
  tips: CravingTip[];
  specialMessage: string;
}

// Authentication Models
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface AuthResponse {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  token: string;
}

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  createdAt: string;
  lastLoginAt: string;
}
