# Use the official .NET 8 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/backend/Pms.Backend.Api/Pms.Backend.Api.csproj", "src/backend/Pms.Backend.Api/"]
COPY ["src/backend/Pms.Backend.Application/Pms.Backend.Application.csproj", "src/backend/Pms.Backend.Application/"]
COPY ["src/backend/Pms.Backend.Domain/Pms.Backend.Domain.csproj", "src/backend/Pms.Backend.Domain/"]
COPY ["src/backend/Pms.Backend.Infrastructure/Pms.Backend.Infrastructure.csproj", "src/backend/Pms.Backend.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/backend/Pms.Backend.Api/Pms.Backend.Api.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/src/backend/Pms.Backend.Api"
RUN dotnet build "Pms.Backend.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Pms.Backend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create logs directory
RUN mkdir -p /app/logs

ENTRYPOINT ["dotnet", "Pms.Backend.Api.dll"]