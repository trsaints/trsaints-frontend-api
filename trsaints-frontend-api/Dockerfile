FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID

ARG JWT_SINGING_KEY
ARG JWT_ISSUER
ARG JWT_AUDIENCE
ARG DB_HOST
ARG DB_PORT
ARG DB_NAME
ARG DB_USER
ARG DB_PASSWORD

ENV JWT_SINGING_KEY=$JWT_SINGING_KEY
ENV JWT_ISSUER=$JWT_ISSUER
ENV JWT_AUDIENCE=$JWT_AUDIENCE
ENV POSTGRES_HOST=$DB_HOST
ENV POSTGRES_PORT=$DB_PORT
ENV POSTGRES_DB=$DB_NAME
ENV POSTGRES_USER=$DB_USER
ENV POSTGRES_PASSWORD=$DB_PASSWORD

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
RUN rm -f /home/app/.microsoft/usersecrets/94a07a34-61c2-4d4b-a62d-b09347301694/secrets.json
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "trsaints-frontend-api.dll"]
