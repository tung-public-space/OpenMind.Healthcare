using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuitSmokingApi.Domain.Repositories;
using QuitSmokingApi.Domain.Services;
using QuitSmokingApi.Features.Achievements;
using QuitSmokingApi.Features.Motivation;
using QuitSmokingApi.Features.Progress;
using QuitSmokingApi.Infrastructure.Data;
using QuitSmokingApi.Infrastructure.Data.Repositories;
using QuitSmokingApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR for vertical slice architecture
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add SQLite Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=quitSmoking.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Register services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserService>(provider => provider.GetRequiredService<UserService>());

// Register repositories (each aggregate root has its own repository)
builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();
builder.Services.AddScoped<IQuitJourneyRepository, QuitJourneyRepository>();
builder.Services.AddScoped<IMotivationalQuoteRepository, MotivationalQuoteRepository>();
builder.Services.AddScoped<ICravingTipRepository, CravingTipRepository>();

// Register domain services
builder.Services.AddScoped<AchievementStatusService>();

// Add CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost", "http://localhost:80", "http://localhost:3004")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

// Map feature endpoints (vertical slices)
app.MapProgressEndpoints();
app.MapAchievementsEndpoints();
app.MapMotivationEndpoints();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}

app.Run();
