# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore ./src/TrustAutomation.Api/TrustAutomation.Api.csproj
RUN dotnet publish ./src/TrustAutomation.Api/TrustAutomation.Api.csproj -c Release -o /app --no-restore

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "TrustAutomation.Api.dll"]