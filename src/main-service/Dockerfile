FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5257

ENV ASPNETCORE_URLS=http://+:5257

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/main-service/main-service.csproj", "src/main-service/"]
RUN dotnet restore "src/main-service/main-service.csproj"
COPY . .
WORKDIR "/src/src/main-service"
RUN dotnet build "main-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "main-service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "main-service.dll"]
