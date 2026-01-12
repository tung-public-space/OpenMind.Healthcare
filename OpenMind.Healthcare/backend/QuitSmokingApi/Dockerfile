# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy shared projects first
COPY Shared/DDD.BuildingBlocks/DDD.BuildingBlocks.csproj Shared/DDD.BuildingBlocks/

# Copy main project file
COPY QuitSmokingApi/QuitSmokingApi.csproj QuitSmokingApi/

# Restore dependencies
RUN dotnet restore QuitSmokingApi/QuitSmokingApi.csproj

# Copy all source code
COPY . .

# Build and publish
WORKDIR /src/QuitSmokingApi
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Create non-root user for security
RUN adduser --disabled-password --gecos "" appuser

# Create directory for SQLite database with proper permissions
RUN mkdir -p /app/data && chown -R appuser:appuser /app/data

COPY --from=build /app/publish .

# Set ownership of app files
RUN chown -R appuser:appuser /app

USER appuser

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5000

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:5000/health || exit 1

ENTRYPOINT ["dotnet", "QuitSmokingApi.dll"]
