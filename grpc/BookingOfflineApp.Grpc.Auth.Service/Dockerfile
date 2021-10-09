#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["grpc/BookingOfflineApp.Grpc.Auth.Service/BookingOfflineApp.Grpc.Auth.Service.csproj", "grpc/BookingOfflineApp.Grpc.Auth.Service/"]
RUN dotnet restore "grpc/BookingOfflineApp.Grpc.Auth.Service/BookingOfflineApp.Grpc.Auth.Service.csproj"
COPY . .
WORKDIR "/src/grpc/BookingOfflineApp.Grpc.Auth.Service"
RUN dotnet build "BookingOfflineApp.Grpc.Auth.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookingOfflineApp.Grpc.Auth.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookingOfflineApp.Grpc.Auth.Service.dll"]