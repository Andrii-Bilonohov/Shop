<div align="center">

# 🛒 Shop

**Навчальний full-stack e-commerce проєкт**

ASP.NET Core бекенд + React фронтенд — авторизація, каталог товарів, кошик.

[![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-Vite-61DAFB?logo=react&logoColor=white)](https://react.dev/)
[![EF Core](https://img.shields.io/badge/EF%20Core-ORM-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/ef/core/)

</div>

---

## ✨ Про проєкт

**Shop** — навчальний pet-проєкт для відпрацювання full-stack розробки: класичний інтернет-магазин з розділенням на бекенд (ASP.NET Core Web API) і фронтенд (React), без прив'язки до конкретної ніші товарів. Мета — практика архітектури, а не продакшн-магазин.

## 🧠 Архітектура

```
┌───────────────────┐        REST API         ┌───────────────────────┐
│      Client/        │ ──────────────────────▶│      Server/            │
│      React (Vite)    │◀─────────────────────  │  ASP.NET Core Web API   │
└───────────────────┘                          └───────────┬─────────────┘
                                                             │
                                                     Entity Framework Core
                                                             │
                                                          Database
```

Фронтенд і бекенд — окремі проєкти в одному репозиторії (`Client/` і `Server/`), спілкуються через REST API.

## 🛠 Технології

| | |
|---|---|
| **Server** | ASP.NET Core Web API · Entity Framework Core |
| **Client** | React · Vite |

## 🚀 Функціонал

- 🔐 Авторизація користувачів
- 📦 Каталог товарів
- 🛒 Кошик покупок

## 📁 Структура проєкту

```
Shop/
├── Client/                 # React-фронтенд (Vite)
│   └── ...
│
└── Server/
    └── src/                # ASP.NET Core Web API
        └── ...
```

## ⚡ Швидкий старт

### Вимоги

- [.NET SDK 8+](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- SQL Server 

### Server

```bash
cd Server/src
dotnet restore
dotnet ef database update   # застосувати міграції
dotnet run
```

За замовчуванням API піднімається на `https://localhost:5001` (або порт, вказаний у `launchSettings.json`).

### Client

```bash
cd Client
npm install
npm run dev
```

Фронтенд буде доступний на `http://localhost:5173` (стандартний порт Vite).

### Змінні оточення / конфігурація

Секрети та локальні налаштування (`appsettings.Development.json`, `.env`) не комітяться в репозиторій — створіть їх локально за зразком, якщо такий є в проєкті, або зверніться до конфігурації `Server/src/appsettings.json` як до базової.


<div align="center">
Pet-проєкт для практики full-stack розробки
</div>
