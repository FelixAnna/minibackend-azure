#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:3.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BookingOfflineApp/BookingOfflineApp.csproj", "BookingOfflineApp/"]
COPY ["BookingOfflineApp.Services/BookingOfflineApp.Services.csproj", "BookingOfflineApp.Services/"]
COPY ["BookingOfflineApp.Common/BookingOfflineApp.Common.csproj", "BookingOfflineApp.Common/"]
COPY ["BookingOfflineApp.Entities/BookingOfflineApp.Entities.csproj", "BookingOfflineApp.Entities/"]
COPY ["BookingOfflineApp.Repositories.SqlServer/BookingOfflineApp.Repositories.SqlServer.csproj", "BookingOfflineApp.Repositories.SqlServer/"]
COPY ["BookingOfflineApp.Repositories/BookingOfflineApp.Repositories.csproj", "BookingOfflineApp.Repositories/"]
RUN dotnet restore "BookingOfflineApp/BookingOfflineApp.csproj"
COPY . .
WORKDIR "/src/BookingOfflineApp"
RUN dotnet build "BookingOfflineApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookingOfflineApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true