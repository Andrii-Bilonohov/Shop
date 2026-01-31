# -------------------------------
# Образ для миграций EF
# -------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS ef-migrate
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY Application/Application.csproj ./Application/
COPY Domain/Domain.csproj ./Domain/
RUN dotnet restore Infrastructure/Infrastructure.csproj

# Копируем весь исходный код
COPY . .

# Точка входа — команда EF
ENTRYPOINT ["dotnet", "ef", "database", "update", "--project", "Infrastructure/Infrastructure.csproj"]
