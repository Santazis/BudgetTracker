FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BudgetTracker.Api/BudgetTracker.Api.csproj", "BudgetTracker.Api/"]
COPY ["BudgetTracker.Infrastructure/BudgetTracker.Infrastructure.csproj", "BudgetTracker.Infrastructure/"]
COPY ["BudgetTracker.Domain/BudgetTracker.Domain.csproj", "BudgetTracker.Domain/"]
COPY ["BudgetTracker.Application/BudgetTracker.Application.csproj", "BudgetTracker.Application/"]
RUN dotnet restore "BudgetTracker.Api/BudgetTracker.Api.csproj"
COPY . .
WORKDIR "/src/BudgetTracker.Api"
RUN dotnet build "./BudgetTracker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BudgetTracker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetTracker.Api.dll"]
