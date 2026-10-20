# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first so this layer is cached while only the code changes
COPY MaintenanceTickets.API/MaintenanceTickets.API.csproj MaintenanceTickets.API/
RUN dotnet restore MaintenanceTickets.API/MaintenanceTickets.API.csproj

COPY . .
RUN dotnet publish MaintenanceTickets.API/MaintenanceTickets.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
# Run as the non-root user provided by the .NET image
USER $APP_UID

# Railway injects PORT; 8080 is the fallback
CMD ["sh", "-c", "dotnet MaintenanceTickets.API.dll --urls http://0.0.0.0:${PORT:-8080}"]