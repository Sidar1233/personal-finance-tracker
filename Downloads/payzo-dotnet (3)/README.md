# 💰 Payzo — ASP.NET Core MVC

Уеб приложение за следене на лични финанси. Стартира с `dotnet run`.

---

## 🚀 Стартиране

### Изисквания
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- (по желание) VS Code с **C# Dev Kit** extension

### Команди

```bash
# Влезте в папката на проекта
cd payzo-dotnet

# Стартирайте
dotnet run

# Отворете браузър на:
# http://localhost:5000
```

### VS Code
1. `File → Open Folder` → изберете `payzo-dotnet/`
2. Инсталирайте препоръчаните extensions (C# Dev Kit)
3. `F5` или Terminal → `dotnet run`

---

## 👤 Акаунти за вход

| Роля | Имейл | Парола |
|------|-------|--------|
| 👑 Супер Администратор | `superadmin@payzo.bg` | `superadmin123` |
| 🔑 Администратор | `admin@payzo.bg` | `admin123` |
| 🔑 Администратор | `g.ivanov@payzo.bg` | `admin456` |
| 👤 Потребител | `ivan@example.bg` | `ivan123` |
| 👤 Потребител | `elena@example.bg` | `elena123` |
| 👤 Потребител | `petar@example.bg` | `petar123` |
| 🔒 Деактивиран | `anna@example.bg` | `anna123` |

---

## 📁 Структура

```
payzo-dotnet/
├── Program.cs                  ← Startup, DI, middleware
├── Payzo.csproj                ← Project file (net8.0)
├── Controllers/
│   ├── AccountController.cs    ← Login, Register, Logout
│   ├── DashboardController.cs  ← Главно табло
│   ├── TransactionsController.cs
│   ├── BudgetGoalsController.cs
│   └── UsersController.cs      ← Admin + Reports + Settings
├── Models/Models.cs            ← User, Transaction, Budget, Goal, ViewModels
├── Data/PayzoDb.cs             ← In-memory store + seed данни
├── Services/
│   ├── UserService.cs          ← BCrypt auth, CRUD
│   └── FinanceService.cs       ← Transactions, Budget, Goals
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   ├── _Sidebar.cshtml     ← Показва само позволените менюта
│   │   └── _Topbar.cshtml
│   ├── Account/Login.cshtml
│   ├── Dashboard/Index.cshtml  ← Chart.js графики
│   ├── Transactions/Index.cshtml
│   ├── Budget/Index.cshtml
│   ├── Goals/Index.cshtml
│   └── Users/
│       ├── Index.cshtml        ← Admin: управление потребители
│       ├── Reports.cshtml      ← Admin: отчети
│       └── Settings.cshtml     ← SuperAdmin: системни настройки
└── wwwroot/
    ├── css/style.css           ← Design system
    ├── js/site.js              ← Modals, toasts
    └── images/logo.png
```

---

## 🔐 Роли и права

| | User | Admin | SuperAdmin |
|---|---|---|---|
| Лични финанси | ✅ | ✅ | ✅ |
| Преглед потребители | ❌ | ✅ | ✅ |
| Редакция потребители | ❌ | ✅ (само User) | ✅ |
| Изтриване потребители | ❌ | ❌ | ✅ |
| Отчети | ❌ | ✅ | ✅ |
| Системни настройки | ❌ | ❌ | ✅ |

---

## 🛠️ Технологии
- **ASP.NET Core 8 MVC** – Controllers, Razor Views, Tag Helpers
- **C#** – Services, Models, BCrypt password hashing
- **Cookie Authentication** – Claims-based, role policies
- **Chart.js** – Интерактивни диаграми
- **In-Memory store** – готов за замяна с EF Core + SQL Server
