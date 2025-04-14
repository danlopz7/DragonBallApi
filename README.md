# 🐉 DragonBall API Integration (.NET MVC)

This project integrates with the [DragonBall API](https://dragonball-api.com) to fetch characters and transformations from the Dragon Ball universe. It selectively stores only characters of race `Saiyan` and affiliation `Z Fighter` into a local SQL Server database.

---

## ⚙️ Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- DragonBall REST API

---

## ✨ Features

- Fetches character and transformation data from an external API
- Conditionally stores only Saiyans and Z Fighters in the database
- Provides RESTful endpoints for syncing, querying, and clearing data
- Uses DTOs for clean and decoupled API responses
- Implements the `Result<T>` pattern for robust error and flow handling

---

## 📌 Available Endpoints

| Method | Route                    | Description                                                    | Authentication |
|--------|-------------------------|----------------------------------------------------------------|----------------|
| POST   | `/api/characters/sync`  | Syncs characters and transformations from the external API     | No             |
| DELETE | `/api/characters/clear` | Clears all characters and transformations from the database    | No             |
| GET    | `/api/characters`       | Returns all characters in the database                         | No             |
| GET    | `/api/characters/{id}`  | Retrieves a specific character by ID                           | No             |

---

## 🔧 Requirements

Make sure you have the following installed before running the project:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server (any edition: LocalDB, Express, Developer, etc.)
- Visual Studio 2022 or Visual Studio Code

---

## 🛠️ Project Setup

### 1. Clone the repository

```bash
git clone https://github.com/danlopz7/DragonBallApi.git
cd DragonBallApi
```

### 2. Create the database manually (if it doesn’t exist)

```sql
CREATE DATABASE DragonBallAPI;
```

### 3. Update the connection string

Open `appsettings.json` and configure your SQL Server credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DragonBallAPI;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}
```

---

## 🗃️ Apply Entity Framework Migrations

Make sure you have EF tools installed:

```bash
dotnet tool install --global dotnet-ef
```

Then apply the existing migrations to set up the database schema:

```bash
dotnet ef database update
```

---

## 📦 Restore Dependencies

```bash
dotnet restore
```

---

## 🚀 Run the Project

```bash
dotnet run
```

Open your browser and navigate to:

```
https://localhost:<your-port>/index.html
```

Swagger UI will let you explore all available endpoints.

---

## 📁 Project Structure

```
DragonBallApi/
│
├── Controllers/         # API Controllers
├── Services/            # Business logic layer
├── Clients/             # External API client
├── DTOs/                # Data Transfer Objects
├── Models/              # Entity models
├── Migrations/          # EF Core migration files
└── appsettings.json     # Project configuration
```

---

## ✅ Best Practices Followed

- Clear separation of concerns (external API, DB, services)
- DTOs for API responses to avoid tight coupling with EF entities
- Dependency injection for services and clients
- Graceful error handling via `Result<T>` wrapper
- `.gitignore` in place to exclude build/output folders
