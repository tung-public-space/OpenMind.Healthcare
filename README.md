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

## Tech Stack

| Layer | Technology |
|-------|------------|
| Backend | .NET 10, ASP.NET Core Minimal APIs |
| Frontend | Angular 19 |
| Database | SQLite with Entity Framework Core |
| Architecture | Domain-Driven Design (DDD), Vertical Slice (Feature-based) |
| Messaging | MediatR |
| Auth | JWT Bearer Authentication |
| API Docs | OpenAPI + Scalar |
| Containerization | Docker & Docker Compose |

## Project Structure

```
OpenMind.Healthcare/
├── backend/
│   ├── QuitSmokingApi/              # Main API service
│   │   ├── Domain/                  # DDD domain layer
│   │   │   ├── Aggregates/          # Aggregate roots (QuitJourney, Achievement, etc.)
│   │   │   ├── Events/              # Domain events
│   │   │   ├── Repositories/        # Repository interfaces
│   │   │   ├── Services/            # Domain services
│   │   │   └── ValueObjects/        # Value objects
│   │   ├── Features/                # Vertical slices by feature
│   │   │   ├── Achievements/        # Achievement feature (endpoints, handlers)
│   │   │   ├── Motivation/          # Motivation feature (quotes, tips)
│   │   │   └── Progress/            # Progress tracking feature
│   │   ├── Infrastructure/          # Data access, EF Core context
│   │   └── Services/                # Application services
│   ├── UserApi/                     # User authentication service
│   │   ├── Domain/                  # User domain
│   │   ├── Features/Auth/           # Authentication endpoints
│   │   └── Infrastructure/          # User database context
│   └── Shared/
│       └── DDD.BuildingBlocks/      # Shared DDD base classes
├── frontend/                        # Angular 19 SPA
│   └── src/app/
│       ├── components/              # UI components
│       ├── services/                # API services
│       ├── models/                  # TypeScript interfaces
│       └── guards/                  # Route guards
└── docker-compose.yml               # Container orchestration
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- Node.js (v18+)
- npm or yarn
- Docker (optional, for containerized deployment)

### Running the Backend

```bash
cd OpenMind.Healthcare/backend/QuitSmokingApi
dotnet restore
dotnet run
```

The API will be available at `http://localhost:5000`

API documentation (Scalar UI): `http://localhost:5000/scalar/v1`

### Running the Frontend

```bash
cd OpenMind.Healthcare/frontend
npm install
npm start
```

The app will be available at `http://localhost:4200`

### Running with Docker

```bash
cd OpenMind.Healthcare
docker-compose up --build
```

Services will be available at:
- **Frontend**: `http://localhost:80`
- **API**: `http://localhost:5000`

## API Endpoints

### Progress
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/progress` | Get user progress |
| `POST` | `/api/progress` | Create/update progress |
| `GET` | `/api/progress/stats` | Get detailed statistics |
| `GET` | `/api/progress/health-milestones` | Get health milestones |

### Achievements
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/achievements` | Get all achievements |
| `GET` | `/api/achievements/unlocked` | Get unlocked achievements |
| `GET` | `/api/achievements/check-new` | Check for newly unlocked achievements |

### Motivation
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/motivation/quote` | Get a random motivational quote |
| `GET` | `/api/motivation/craving-tips` | Get craving tips |
| `GET` | `/api/motivation/daily` | Get daily encouragement |

## Architecture

This project follows **Domain-Driven Design (DDD)** principles with a **Vertical Slice Architecture**:

- **Domain Layer**: Contains aggregates, entities, value objects, domain events, and repository interfaces
- **Features**: Organized by business capability (Progress, Achievements, Motivation) with each feature containing its own endpoints and handlers
- **Infrastructure**: Database context, repository implementations, and external service integrations
- **Shared Building Blocks**: Reusable DDD base classes (`AggregateRoot`, `Entity`, `ValueObject`, `IDomainEvent`)

## Congratulations! 🎉

You're on your smoke-free journey! Every day gets easier, and your body is already healing. You've got this! 💪
