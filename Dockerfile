# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copia tudo
COPY . .

# restaura e publica a partir do csproj
RUN dotnet restore ./TrustAutomation.Api/TrustAutomation.Api.csproj
RUN dotnet publish ./TrustAutomation.Api/TrustAutomation.Api.csproj -c Release -o /app

# run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "TrustAutomation.Api.dll"]
