# Utiliza la imagen de SDK de .NET 9 para construir
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia los archivos del proyecto y restaura dependencias
COPY . .
RUN dotnet restore "EMSI_Corporation/EMSI_Corporation.csproj"

# Construye el proyecto
RUN dotnet build "EMSI_Corporation/EMSI_Corporation.csproj" -c Release -o /app/build

# Publica la aplicación
FROM build AS publish
RUN dotnet publish "EMSI_Corporation/EMSI_Corporation.csproj" -c Release -o /app/publish

# Utiliza la imagen de tiempo de ejecución de ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EMSI_Corporation.dll"]