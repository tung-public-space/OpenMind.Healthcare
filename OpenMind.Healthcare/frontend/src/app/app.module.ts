import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SetupComponent } from './components/setup/setup.component';
import { AchievementsComponent } from './components/achievements/achievements.component';
import { HealthMilestonesComponent } from './components/health-milestones/health-milestones.component';
import { MotivationComponent } from './components/motivation/motivation.component';
import { CravingHelpComponent } from './components/craving-help/craving-help.component';
import { StatsCardComponent } from './components/stats-card/stats-card.component';
import { NavbarComponent } from './components/navbar/navbar.component';

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'setup', component: SetupComponent },
  { path: 'achievements', component: AchievementsComponent },
  { path: 'health', component: HealthMilestonesComponent },
  { path: 'motivation', component: MotivationComponent },
  { path: 'craving-help', component: CravingHelpComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    SetupComponent,
    AchievementsComponent,
    HealthMilestonesComponent,
    MotivationComponent,
    CravingHelpComponent,
    StatsCardComponent,
    NavbarComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
