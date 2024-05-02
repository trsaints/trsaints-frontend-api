FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID

ARG POSTGRES_HOST
ARG POSTGRES_PORT
ARG POSTGRES_DB
ARG POSTGRES_USER
ARG POSTGRES_PASSWORD

ENV POSTGRES_HOST=$POSTGRES_HOST
ENV POSTGRES_PORT=$POSTGRES_PORT
ENV POSTGRES_DB=$POSTGRES_DB
ENV POSTGRES_USER=$POSTGRES_USER
ENV POSTGRES_PASSWORD=$POSTGRES_PASSWORD

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["trsaints-frontend-api/trsaints-frontend-api.csproj", "trsaints-frontend-api/"]
RUN dotnet restore "trsaints-frontend-api/trsaints-frontend-api.csproj"
COPY . .

WORKDIR "/src/trsaints-frontend-api"
RUN dotnet build "trsaints-frontend-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "trsaints-frontend-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "trsaints-frontend-api.dll"]
