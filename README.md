# Expense.Logger

[![Build](https://github.com/Felipelv199/Expense.Logger/actions/workflows/build.yml/badge.svg)](https://github.com/Felipelv199/Expense.Logger/actions/workflows/build.yml)

A personal expense tracker. The goal is to record income, expenses and transfers, organise them by category and bank account, and eventually keep spending inside a user-defined budget.

This repository is the current version: an ASP.NET Core (.NET 10) Web API with a layered architecture, plus a Vite + React client that is not built yet. It is a work in progress; see [Status](#status-and-roadmap) for what exists today.

## Architecture

The solution (`Expense.Logger.slnx`) is split into three C# projects and one front-end project:

| Project | Responsibility |
| --- | --- |
| `Expense.Logger.Api` | ASP.NET Core controllers (thin HTTP handlers), a global exception filter that turns business exceptions into consistent JSON error responses, and DI wiring in `Program.cs`. |
| `Expense.Logger.Business` | Handlers with the application logic, validators, business models and custom exceptions, and extension-method mappers between business models and data entities. |
| `Expense.Logger.Data` | EF Core `DbContext` on the MySQL provider (`MySql.EntityFrameworkCore`), entities (`Transactions`, `Categories`, `BankAccounts`), repositories and migrations. |
| `expense.logger.client` | Vite + React + TypeScript client. Currently still the project template. |

Requests flow `Controller → Handler (validation + rules) → Repository → MySQL`. Controllers contain no try/catch; handlers throw business exceptions and the exception filter maps them to HTTP status codes (`404` for a missing transaction, `400` for invalid input or an unknown category).

## API

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/transactions` | Create a transaction. Body: `name`, `amount`, `date`, `type` (`Expense`, `Income` or `Transfer`), optional `description` and `categoryId`. The date cannot be in the future and the category must exist. |
| `GET` | `/transactions/{id}` | Get one transaction, including its category and bank account. Returns `404` if it does not exist. |
| `GET` | `/transactions` | List transactions, paged and filtered. |

Query parameters for `GET /transactions`:

- `pageNumber` (default `1`) and `pageSize` (default `10`)
- `startDate`, `endDate`
- `minAmount`, `maxAmount`
- `type`
- `search` (matches name or description)

## Running locally

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MySQL 8 (or compatible) running locally
- Node.js and npm (the API starts the Vite dev server through the SPA proxy)
- The EF Core CLI: `dotnet tool install --global dotnet-ef`

### 1. Configure the connection string

The connection string is not stored in the repository; `appsettings.json` only has an empty placeholder. For development, store it with .NET user secrets:

```bash
cd Expense.Logger.Api
dotnet user-secrets set "ConnectionStrings:ExpenseLoggerDatabase" "server=localhost;database=ExpenseLoggerDB;user=<user>;password=<password>"
```

Outside development (or instead of user secrets), set the environment variable `ConnectionStrings__ExpenseLoggerDatabase`. The API refuses to start if neither is set.

### 2. Create the database

```bash
cd Expense.Logger.Api
dotnet ef database update --project ../Expense.Logger.Data
```

More on migrations in [DatabaseMigrations.md](DatabaseMigrations.md).

### 3. Run

```bash
cd expense.logger.client
npm install
```

```bash
cd Expense.Logger.Api
dotnet run
```

The API listens on `https://localhost:7232` / `http://localhost:5298`; the client dev server runs on `https://localhost:60193`. In development the OpenAPI document is served at `/openapi/v1.json`.

## Status and roadmap

Done:

- Create, get by id, and paged/filtered listing of transactions
- Validation and consistent error responses
- EF Core schema and migrations for transactions, categories and bank accounts
- CI build on GitHub Actions

Not done yet:

- Update and delete transactions (`UpdateAsync` / `DeleteAsync` still throw `NotImplementedException`)
- Endpoints for categories and bank accounts
- Budgets
- The React UI
- Automated tests
- Authentication

## History

This is a rewrite. The first version was an Express + MySQL API using raw SQL ([expense-logger-api](https://github.com/Felipelv199/expense-logger-api)) with a Next.js front end ([expense-logger-web](https://github.com/Felipelv199/expense-logger-web)). This version moves the back end to .NET with a layered architecture and EF Core migrations.

## License

[MIT](LICENSE)
