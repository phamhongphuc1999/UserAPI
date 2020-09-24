FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster
COPY ./ ./src
WORKDIR /src
RUN dotnet restore UserAPI/UserAPI.csproj
RUN dotnet restore MongoDatabase/MongoDatabase.csproj
RUN dotnet build UserAPI/UserAPI.csproj -c Release -o /app/build
RUN dotnet build MongoDatabase/MongoDatabase.csproj -c Release -o /app/build
RUN dotnet publish UserAPI/UserAPI.csproj -c Release -o /app/publish
RUN dotnet publish MongoDatabase/MongoDatabase.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserAPI.dll"]
