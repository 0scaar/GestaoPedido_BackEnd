FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MF.OrderManagement.Api/MF.OrderManagement.Api.csproj", "src/MF.OrderManagement.Api/"]
RUN dotnet restore "src/MF.OrderManagement.Api/MF.OrderManagement.Api.csproj"
COPY . .
WORKDIR "/src/src/MF.OrderManagement.Api"
RUN dotnet build "./MF.OrderManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MF.OrderManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MF.OrderManagement.Api.dll"]
