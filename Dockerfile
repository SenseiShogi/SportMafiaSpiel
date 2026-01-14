# ------------------------------
# 1. Build-Stage
# ------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# ------------------------------
# Projektdateien kopieren und Abhängigkeiten wiederherstellen
# ------------------------------
COPY *.csproj ./
RUN dotnet restore

# ------------------------------
# gesamten Code kopieren und veröffentlichen
# ------------------------------
COPY . ./
RUN dotnet publish -c Release -o /app/out

# ------------------------------
# 2. Runtime-Stage
# ------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# ------------------------------
# Build-Ergebnisse kopieren
# ------------------------------
COPY --from=build /app/out .

# ------------------------------
# Container-Umgebung einstellen
# ------------------------------
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

# ------------------------------
# Render-Port freigeben
# ------------------------------
EXPOSE 10000

# ------------------------------
# Anwendung starten
# ------------------------------
ENTRYPOINT ["dotnet", "SportMafiaSpiel.dll"]
