# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Personal expense tracker: an ASP.NET Core (.NET 10) Web API with a layered architecture on MySQL via EF Core, plus a Vite + React + TypeScript client (`expense.logger.client`) that is still the Visual Studio template (a "Weather forecast" page calling `/weatherforecast`, an endpoint the API no longer has). Update/delete transactions, category and bank-account endpoints, budgets, the real UI, auth and tests are not built yet.

## Commands

Backend (run from the repo root unless noted):

```bash
dotnet restore Expense.Logger.slnx
dotnet build Expense.Logger.slnx --configuration Release   # what CI runs (.github/workflows/build.yml)
cd Expense.Logger.Api && dotnet run                        # https://localhost:7232, http://localhost:5298
```

`dotnet run` also starts the Vite dev server through the SPA proxy (`npm run dev`, https://localhost:60193), so run `npm install` in `expense.logger.client` first. OpenAPI is at `/openapi/v1.json` in Development. `Expense.Logger.Api/Expense.Logger.Server.http` has sample requests.

The API refuses to start without the `ExpenseLoggerDatabase` connection string. `appsettings.json` has only an empty placeholder; set it with user secrets or an env var:

```bash
cd Expense.Logger.Api
dotnet user-secrets set "ConnectionStrings:ExpenseLoggerDatabase" "server=localhost;database=ExpenseLoggerDB;user=<user>;password=<password>"
# or: ConnectionStrings__ExpenseLoggerDatabase=...
```

EF Core migrations (needs `dotnet tool install --global dotnet-ef`; always run from `Expense.Logger.Api` with `--project` pointing at the Data project, where migrations live):

```bash
cd Expense.Logger.Api
dotnet ef migrations add <Name> --project ../Expense.Logger.Data
dotnet ef database update --project ../Expense.Logger.Data
```

Client (`expense.logger.client`): `npm run build` (`tsc -b && vite build`), `npm run lint`. There are no tests anywhere in the repo yet.

## Architecture

Request flow: `Controller (Api) → Handler (Business) → Repository (Data) → MySQL`.

- **Api**: controllers are one-line pass-throughs to handlers with no try/catch. `Filters/HttpResponseExceptionFilter.cs` is a global filter that turns exceptions into an `ErrorResponse` JSON body. Status codes come from a `switch` on exception type (`TransactionNotFound` → 404; `TransactionCategoryNotFound`, `InvalidTransactionCreateException`, `InvalidTransactionQueryException` → 400; anything else → 500 with a generic message). **A new business exception must be added to that switch**, or it surfaces as a 500.
- **Business**: handlers (`TransactionHandler` is a `partial` class; rule helpers like `EnsureTransactionCategoryExists` live in `TransactionHandler.Rules.cs`) call static validators in `Validators/`, which throw exceptions deriving from `InvalidDataException : BusinessException`; these carry field details that become the `errors` array in 400 responses. Business models and data entities are separate types, converted with extension methods in `Mappers/` (`ToDataModel()`, `ToBusinessModel()`, `ToFilter()`, `ToPagination()`).
- **Data**: entities use plural names (`Transactions`, `Categories`, `BankAccounts`) and inherit DB-generated `CreatedAt`/`UpdatedAt` from `BaseDataModel`. Category and bank account foreign keys on a transaction are optional with `SET NULL` on delete. Repositories (note the existing `Catgories` spelling in type names) are registered as **singletons** and take `IDbContextFactory<ExpenseLoggerDbContext>` (pooled), creating a short-lived context per method with `using var context = await _contextFactory.CreateDbContextAsync()`. Never inject `DbContext` directly into them.

Dependency injection is wired by hand in `Expense.Logger.Api/Program.cs`; register new handlers/repositories there as singletons following the same pattern.

## C# conventions enforced as build errors (`.editorconfig`)

- File-scoped namespaces.
- Namespace must match folder structure (IDE0130).
- No unnecessary `using` directives (IDE0005).
- Prefer primary constructors (the existing classes use them, then assign the parameter to a public field such as `_transactionsHandler`).

## Client deployment (Vercel)

The client is set up to be served at `felipelopez.vercel.app/expense-logger/` through a rewrite in the separate `portfolio` repo. `vite.config.ts` uses `base: '/expense-logger/'` for builds only, and generates the ASP.NET HTTPS dev certificate (via the `dotnet` CLI) only for the dev server, since Vercel has no `dotnet`. Keep both of those conditional. `expense.logger.client/vercel.json` maps `/expense-logger/*` back to `/*` so the deployment also works on its own domain. The .NET API can't run on Vercel.
