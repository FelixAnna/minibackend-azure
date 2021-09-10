#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BookingOfflineAPI/BookingOfflineApp.Web.csproj", "BookingOfflineAPI/"]
COPY ["BookingOfflineApp.Services/BookingOfflineApp.Services.csproj", "BookingOfflineApp.Services/"]
COPY ["BookingOfflineApp.Common/BookingOfflineApp.Common.csproj", "BookingOfflineApp.Common/"]
COPY ["BookingOfflineApp.Entities/BookingOfflineApp.Entities.csproj", "BookingOfflineApp.Entities/"]
COPY ["BookingOfflineApp.Repositories.SqlServer/BookingOfflineApp.Repositories.SqlServer.csproj", "BookingOfflineApp.Repositories.SqlServer/"]
COPY ["BookingOfflineApp.Repositories/BookingOfflineApp.Repositories.csproj", "BookingOfflineApp.Repositories/"]
RUN dotnet restore "BookingOfflineAPI/BookingOfflineApp.Web.csproj"
COPY . .
WORKDIR "/src/BookingOfflineAPI"
RUN dotnet build "BookingOfflineApp.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookingOfflineApp.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookingOfflineApp.Web.dll"]