import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-setup',
  standalone: false,
  template: `
    <div class="setup-page fade-in">
      <div class="card setup-card">
        <div class="setup-header">
          <h1>🌟 Let's Begin Your Journey</h1>
          <p>Please provide some information to personalize your experience</p>
        </div>

        <form [formGroup]="setupForm" (ngSubmit)="onSubmit()">
          <div class="form-group">
            <label for="quitDate">When did you quit smoking?</label>
            <input 
              type="datetime-local" 
              id="quitDate" 
              formControlName="quitDate"
              [max]="maxDate">
            <span class="hint">Select the date and time you had your last cigarette</span>
          </div>

          <div class="form-group">
            <label for="cigarettesPerDay">How many cigarettes did you smoke per day?</label>
            <input 
              type="number" 
              id="cigarettesPerDay" 
              formControlName="cigarettesPerDay"
              min="1" 
              max="100"
              placeholder="e.g., 20">
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="pricePerPack">Price per pack ($)</label>
              <input 
                type="number" 
                id="pricePerPack" 
                formControlName="pricePerPack"
                min="0.01" 
                step="0.01"
                placeholder="e.g., 10.00">
            </div>

            <div class="form-group">
              <label for="cigarettesPerPack">Cigarettes per pack</label>
              <input 
                type="number" 
                id="cigarettesPerPack" 
                formControlName="cigarettesPerPack"
                min="1" 
                max="50"
                placeholder="e.g., 20">
            </div>
          </div>

          <div class="motivation-section">
            <h3>💪 Remember Why You're Doing This</h3>
            <ul class="reasons-list">
              <li>🫀 Better heart and lung health</li>
              <li>💰 More money in your pocket</li>
              <li>👨‍👩‍👧‍👦 More time with loved ones</li>
              <li>🏃 Improved energy and fitness</li>
              <li>😤 Freedom from addiction</li>
              <li>🌟 A longer, healthier life</li>
            </ul>
          </div>

          <button 
            type="submit" 
            class="btn btn-primary submit-btn"
            [disabled]="setupForm.invalid || isSubmitting">
            {{ isSubmitting ? 'Starting...' : 'Start My Smoke-Free Life! 🚀' }}
          </button>
        </form>
      </div>
    </div>
  `,
  styles: [`
    .setup-page {
      max-width: 600px;
      margin: 40px auto;
      padding: 20px;
    }

    .setup-card {
      padding: 40px;
    }

    .setup-header {
      text-align: center;
      margin-bottom: 40px;
      
      h1 {
        font-size: 28px;
        margin-bottom: 10px;
        background: linear-gradient(135deg, #10b981, #3b82f6);
        -webkit-background-clip: text;
        -webkit-text-fill-color: transparent;
      }
      
      p {
        color: rgba(255, 255, 255, 0.7);
      }
    }

    .form-group {
      margin-bottom: 25px;
      
      label {
        display: block;
        margin-bottom: 8px;
        font-weight: 500;
        color: rgba(255, 255, 255, 0.9);
      }
      
      input {
        width: 100%;
        padding: 14px 16px;
        font-size: 16px;
      }
      
      .hint {
        display: block;
        margin-top: 5px;
        font-size: 12px;
        color: rgba(255, 255, 255, 0.5);
      }
    }

    .form-row {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 20px;
    }

    .motivation-section {
      margin: 30px 0;
      padding: 25px;
      background: linear-gradient(135deg, rgba(16, 185, 129, 0.1), rgba(59, 130, 246, 0.1));
      border-radius: 16px;
      border: 1px solid rgba(16, 185, 129, 0.2);
      
      h3 {
        margin-bottom: 15px;
        color: #10b981;
      }
    }

    .reasons-list {
      list-style: none;
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 10px;
      
      li {
        padding: 8px 12px;
        background: rgba(255, 255, 255, 0.05);
        border-radius: 8px;
        font-size: 14px;
      }
    }

    .submit-btn {
      width: 100%;
      padding: 18px;
      font-size: 18px;
      margin-top: 20px;
      
      &:disabled {
        opacity: 0.6;
        cursor: not-allowed;
      }
    }

    @media (max-width: 480px) {
      .form-row {
        grid-template-columns: 1fr;
      }
      
      .reasons-list {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class SetupComponent {
  setupForm: FormGroup;
  isSubmitting = false;
  maxDate: string;

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private router: Router
  ) {
    this.maxDate = new Date().toISOString().slice(0, 16);
    
    // Default quit date: 2 weeks ago (since user mentioned they're in week 2)
    const twoWeeksAgo = new Date();
    twoWeeksAgo.setDate(twoWeeksAgo.getDate() - 14);
    
    this.setupForm = this.fb.group({
      quitDate: [twoWeeksAgo.toISOString().slice(0, 16), Validators.required],
      cigarettesPerDay: [20, [Validators.required, Validators.min(1), Validators.max(100)]],
      pricePerPack: [10.00, [Validators.required, Validators.min(0.01)]],
      cigarettesPerPack: [20, [Validators.required, Validators.min(1), Validators.max(50)]]
    });
  }

  onSubmit(): void {
    if (this.setupForm.valid) {
      this.isSubmitting = true;
      
      const formValue = this.setupForm.value;
      const progress = {
        quitDate: new Date(formValue.quitDate).toISOString(),
        cigarettesPerDay: formValue.cigarettesPerDay,
        pricePerPack: formValue.pricePerPack,
        cigarettesPerPack: formValue.cigarettesPerPack
      };

      this.apiService.saveProgress(progress).subscribe({
        next: () => {
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          console.error('Error saving progress:', err);
          this.isSubmitting = false;
        }
      });
    }
  }
}
