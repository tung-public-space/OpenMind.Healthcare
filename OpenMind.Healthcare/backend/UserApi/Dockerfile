# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy shared projects first
COPY Shared/DDD.BuildingBlocks/DDD.BuildingBlocks.csproj Shared/DDD.BuildingBlocks/

# Copy main project file
COPY UserApi/UserApi.csproj UserApi/

# Restore dependencies
RUN dotnet restore UserApi/UserApi.csproj

# Copy all source code
COPY . .

# Build and publish
WORKDIR /src/UserApi
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Create non-root user for security
RUN useradd -m -s /bin/bash appuser

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

ENTRYPOINT ["dotnet", "UserApi.dll"]
