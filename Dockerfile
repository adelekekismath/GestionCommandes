# ÉTAPE 1 : Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. On copie d'abord les fichiers de solution et de projets pour restaurer les packages
COPY ["tpGestionCommandes.sln", "./"]
COPY ["Api/Api.csproj", "Api/"]
COPY ["Api.Application/Api.Application.csproj", "Api.Application/"]
COPY ["Api.Databases/Api.Databases.csproj", "Api.Databases/"]
COPY ["Api.Domain/Api.Domain.csproj", "Api.Domain/"]
COPY ["Api.ViewModel/Api.ViewModel.csproj", "Api.ViewModel/"]

# 2. Restauration (on pointe vers le fichier .sln à la racine de /src)
RUN dotnet restore "tpGestionCommandes.sln"

# 3. On copie TOUT le reste du code source
COPY . .

# 4. Compilation (Notez le chemin relatif vers le projet Api)
RUN dotnet build "Api/Api.csproj" -c Release -o /app/build

# 5. Publication
FROM build AS publish
RUN dotnet publish "Api/Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ÉTAPE 2 : Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Vérifiez bien si votre DLL de sortie est "Api.dll" ou "tpGestionCommandes.Api.dll"
ENTRYPOINT ["dotnet", "Api.dll"]