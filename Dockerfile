FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY modisette.csproj ./
COPY PostgresMigration/Modisette.PostgresMigration.csproj PostgresMigration/
COPY Modisette.Test/Modisette.Test.csproj Modisette.Test/
RUN dotnet restore modisette.csproj

COPY . .
RUN dotnet publish modisette.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "modisette.dll"]