# Quit Smoking Tracker

🚭 A motivational web application to help you on your smoke-free journey!

## Features

- 📊 **Dashboard** - Track your progress with real-time statistics
- 💰 **Money Saved** - See how much money you've saved
- 🚬 **Cigarettes Not Smoked** - Track the cigarettes you've avoided
- ❤️ **Health Milestones** - Watch your body heal over time
- 🏆 **Achievements** - Unlock badges as you reach milestones
- 💪 **Motivation** - Daily quotes and encouragement
- 🆘 **Craving Help** - Tools to help you beat cravings

## Project Structure

```
quit-smoking-app/
├── backend/                    # C# ASP.NET Core API
│   └── QuitSmokingApi/
│       ├── Controllers/        # API endpoints
│       ├── Models/             # Data models
│       ├── Services/           # Business logic
│       └── Data/               # Database context
├── frontend/                   # Angular application
│   └── src/
│       └── app/
│           ├── components/     # UI components
│           ├── services/       # API services
│           └── models/         # TypeScript interfaces
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js (v16+)
- npm

### Running the Backend

```bash
cd backend/QuitSmokingApi
dotnet restore
dotnet run
```

The API will be available at `http://localhost:5000`

### Running the Frontend

```bash
cd frontend
npm install
npm start
```

The app will be available at `http://localhost:4200`

## API Endpoints

- `GET /api/progress` - Get user progress
- `POST /api/progress` - Create/update progress
- `GET /api/progress/stats` - Get detailed statistics
- `GET /api/progress/health-milestones` - Get health milestones
- `GET /api/achievements` - Get all achievements
- `GET /api/motivation/quote` - Get a random quote
- `GET /api/motivation/craving-tips` - Get craving tips
- `GET /api/motivation/daily` - Get daily encouragement

## Congratulations! 🎉

You're in your second week smoke-free! That's an incredible achievement. Keep going - every day gets easier, and your body is already healing. You've got this! 💪
