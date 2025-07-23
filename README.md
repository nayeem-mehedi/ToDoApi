# ToDoApi

A simple and secure ASP.NET Core Web API to manage TODO items using **JWT authentication** and **Swagger UI** for testing.

Built with **.NET 8**, this project demonstrates modern Web API practices using in-memory auth and a minimal setup — perfect for learning or as a base for a production-grade system.

---

## 🚀 Features

- ✅ Full CRUD API for `TodoItem`s
- 🧾 User registration and login
- 🔐 JWT-based user authentication
- 🔐 Role-based endpoint authorization
- 🧪 Built-in Swagger UI with JWT Bearer support
- ⚙️ Easily configurable via `appsettings.json` + `launchSettings.json`
- 🗄️ MSSQL database support

---

## 🛠️ Tech Stack

- .NET 8.0
- ASP.NET Core Web API
- C#
- JWT Bearer Authentication (`System.IdentityModel.Tokens.Jwt`)
- Swagger UI via `Swashbuckle.AspNetCore`
- Microsoft SQL Server (MSSQL)

---

## 📦 Getting Started

### 🔧 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Rider, Visual Studio, or VS Code
- Terminal / Command Prompt

### ▶️ Run the App

```bash
dotnet restore
dotnet run
```

#### Swagger UI
https://localhost:5001/swagger


## Phase 2:

### 1. Add Database support:
Run DB migration scripts:
1. initial : `dotnet ef migrations add InitialCreate`
2. Future tables add : `dotnet ef migrations add AddToDoItemTable` + `dotnet ef migrations add AddUserTable` 
3. followed by `dotnet ef database update`


