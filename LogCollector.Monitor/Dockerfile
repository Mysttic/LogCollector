#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LogCollector.Monitor/LogCollector.Monitor.csproj", "LogCollector.Monitor/"]
COPY ["LogCollector.Domain/LogCollector.Domain.csproj", "LogCollector.Domain/"]
COPY ["LogCollector.Entities/LogCollector.Entities.csproj", "LogCollector.Entities/"]
COPY ["LogCollector.ServiceDefaults/LogCollector.ServiceDefaults.csproj", "LogCollector.ServiceDefaults/"]
RUN dotnet restore "./LogCollector.Monitor/LogCollector.Monitor.csproj"
COPY . .
WORKDIR "/src/LogCollector.Monitor"
RUN dotnet build "./LogCollector.Monitor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LogCollector.Monitor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LogCollector.Monitor.dll"]