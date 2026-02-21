
# ESTÁGIO 1: COMPILAÇÃO (build)

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar os arquivos .csproj primeiro (para cache de pacotes)
COPY ProductManager.Domain/ProductManager.Domain.csproj ProductManager.Domain/
COPY ProductManager.Application/ProductManager.Application.csproj ProductManager.Application/
COPY ProductManager.Infrastructure/ProductManager.Infrastructure.csproj ProductManager.Infrastructure/
COPY ProductManager.API/ProductManager.API.csproj ProductManager.API/

# Restaurar pacotes NuGet
RUN dotnet restore ProductManager.API/ProductManager.API.csproj

# Copiar todo o código fonte
COPY . .

# Compilar e publicar a API
RUN dotnet publish ProductManager.API/ProductManager.API.csproj -c Release -o /app/publish


# ESTÁGIO 2: EXECUÇÃO (runtime)

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Criar pasta de uploads
RUN mkdir -p /app/Uploads

# Copiar o resultado da compilação do estágio anterior
COPY --from=build /app/publish .

# Expor a porta da API
EXPOSE 8080

# Comando para iniciar a API
ENTRYPOINT ["dotnet", "ProductManager.API.dll"]
