# ToDoApi

A simple and secure ASP.NET Core Web API to manage TODO items using **JWT authentication** and **Swagger UI** for testing.

Built with **.NET 8**, this project demonstrates modern Web API practices using in-memory auth and a minimal setup — perfect for learning or as a base for a production-grade system.

---

## 🚀 Features

- ✅ CRUD API for `TodoItem`s
- 🔐 JWT-based in-memory user authentication
- 🧾 Token generation via `/api/auth/login`
- 🔐 Secured endpoints using `[Authorize]`
- 🧪 Built-in Swagger UI with JWT Bearer support
- ⚙️ Easily configurable via `appsettings.json`

---

## 🛠️ Tech Stack

- .NET 8.0
- ASP.NET Core Web API
- C#
- JWT Bearer Authentication (`System.IdentityModel.Tokens.Jwt`)
- Swagger UI via `Swashbuckle.AspNetCore`

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



