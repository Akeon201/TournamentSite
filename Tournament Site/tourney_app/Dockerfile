FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["tourney_app/tourney_app.csproj", "tourney_app/"]
RUN dotnet restore "tourney_app/tourney_app.csproj"
COPY . .
WORKDIR "/src/tourney_app"
RUN dotnet build "tourney_app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "tourney_app.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "tourney_app.dll"]
