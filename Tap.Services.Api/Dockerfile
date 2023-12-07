#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Tap.Services.Api/Tap.Services.Api.csproj", "Tap.Services.Api/"]
COPY ["Tap.Infrastructure/Tap.Infrastructure.csproj", "Tap.Infrastructure/"]
COPY ["Tap.Application/Tap.Application.csproj", "Tap.Application/"]
COPY ["Tap.Contracts/Tap.Contracts.csproj", "Tap.Contracts/"]
COPY ["Tap.Domain/Tap.Domain.csproj", "Tap.Domain/"]
COPY ["Tap.Persistence/Tap.Persistence.csproj", "Tap.Persistence/"]
RUN dotnet restore "Tap.Services.Api/Tap.Services.Api.csproj"
COPY . .
WORKDIR "/src/Tap.Services.Api"
RUN dotnet build "Tap.Services.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tap.Services.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tap.Services.Api.dll"]