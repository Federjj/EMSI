# Usar la imagen base de SDK para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto y restaurar dependencias
COPY ["EMSI_Corporation/EMSI_Corporation.csproj", "EMSI_Corporation/"]
RUN dotnet restore "EMSI_Corporation/EMSI_Corporation.csproj"

# Copiar todo el código fuente y compilar
COPY . .
WORKDIR "/src/EMSI_Corporation"
RUN dotnet build "EMSI_Corporation.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "EMSI_Corporation.csproj" -c Release -o /app/publish

# Imagen final de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EMSI_Corporation.dll"]