# Etap 1: Budowanie backendu .NET Aspire
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Skopiowanie plików projektu i przywrócenie zależności
COPY ["LogCollector.AppHost/LogCollector.AppHost.csproj", "LogCollector.AppHost/"]
COPY ["LogCollector.Browser/LogCollector.Browser.Server/LogCollector.Browser.Server.csproj", "LogCollector.Browser/LogCollector.Browser.Server/"]
COPY ["LogCollector.DockerManager/LogCollector.DockerManager.csproj", "LogCollector.DockerManager/"]
COPY ["LogCollector.Monitor/LogCollector.Monitor.csproj", "LogCollector.Monitor/"]
COPY ["LogCollector.Service/LogCollector.Service.csproj", "LogCollector.Service/"]
COPY ["LogCollector.Domain/LogCollector.Domain.csproj", "LogCollector.Domain/"]
COPY ["LogCollector.Entities/LogCollector.Entities.csproj", "LogCollector.Entities/"]
COPY ["LogCollector.ServiceDefaults/LogCollector.ServiceDefaults.csproj", "LogCollector.ServiceDefaults/"]

# Ustawienie lokalizacji, gdzie znajduje się projekt AppHost
WORKDIR /src/LogCollector.AppHost

# Instalacja wymaganych workloads
RUN dotnet workload restore

RUN dotnet restore "LogCollector.AppHost.csproj"

# Skopiowanie pozostałych plików projektu
COPY . .

# Budowanie aplikacji backendowej
RUN dotnet build "LogCollector.AppHost.csproj" -c Release -o /app/build

# Publikowanie aplikacji backendowej
FROM build AS publish
RUN dotnet publish "LogCollector.AppHost.csproj" -c Release -o /app/publish
