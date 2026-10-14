# CivicConnect API - repeatable container build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CivicConnect/Domain/Domain.csproj CivicConnect/Domain/
COPY CivicConnect/Application/Application.csproj CivicConnect/Application/
COPY CivicConnect/Infrastructure/Infrastructure.csproj CivicConnect/Infrastructure/
COPY CivicConnect/WebApi/WebApi.csproj CivicConnect/WebApi/

RUN dotnet restore CivicConnect/WebApi/WebApi.csproj
COPY . .
RUN dotnet publish CivicConnect/WebApi/WebApi.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_EnableDiagnostics=0

COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "WebApi.dll"]
