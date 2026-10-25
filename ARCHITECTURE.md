# Architecture and data flow

Diagrams are [Mermaid](https://mermaid.js.org/) and render on GitHub. They describe the code as it is today: only `POST /transactions`, `GET /transactions/{id}` and `GET /transactions` exist, and the React client is still the Visual Studio template.

## 1. System overview

```mermaid
flowchart LR
    subgraph Browser
        Client["expense.logger.client<br/>Vite + React + TS<br/>(template only)"]
    end

    subgraph API["Expense.Logger.Api (ASP.NET Core, .NET 10)"]
        Ctrl["TransactionsController"]
        Filter["HttpResponseExceptionFilter<br/>(global)"]
    end

    subgraph Biz["Expense.Logger.Business"]
        Handler["TransactionHandler"]
        Val["TransactionValidators"]
        Map["Mappers<br/>(extension methods)"]
    end

    subgraph Data["Expense.Logger.Data"]
        Repo["TransactionsRepository<br/>CatgoriesRepository"]
        Ctx["ExpenseLoggerDbContext<br/>(pooled factory)"]
    end

    DB[("MySQL<br/>ExpenseLoggerDB")]

    Client -- "HTTPS /transactions<br/>(dev: Vite proxy)" --> Ctrl
    Ctrl -- "calls" --> Handler
    Handler -- "validate" --> Val
    Handler -- "convert models" --> Map
    Handler -- "reads / writes" --> Repo
    Repo -- "CreateDbContextAsync()" --> Ctx
    Ctx -- "EF Core, MySql provider" --> DB
    Handler -. "throws BusinessException" .-> Filter
    Filter -. "ErrorResponse JSON<br/>404 / 400 / 500" .-> Client
```

Project references point downward: `Api → Business → Data`. `Business` never sees HTTP types, and `Data` never sees business models.

## 2. Request lifecycle

Controllers have no try/catch. A handler either returns a business model or throws, and the exception filter turns the throw into the HTTP response.

```mermaid
sequenceDiagram
    autonumber
    participant C as Client
    participant Ctrl as TransactionsController
    participant H as TransactionHandler
    participant V as TransactionValidators
    participant R as Repositories
    participant DB as MySQL
    participant F as ExceptionFilter

    C->>Ctrl: HTTP request
    Ctrl->>H: handler call (create / get / list)
    H->>V: validate input
    alt invalid input
        V-->>H: throws Invalid*Exception
        H-->>F: exception propagates
        F-->>C: 400 + ErrorResponse
    else valid
        H->>R: repository call
        R->>DB: new DbContext per call, run query
        DB-->>R: rows
        R-->>H: Data entities
        H->>H: ToBusinessModel()
        H-->>Ctrl: business model
        Ctrl-->>C: 200 OK + JSON
    end
```

## 3. Per-endpoint flow

### `POST /transactions`

```mermaid
flowchart TD
    A["POST /transactions<br/>TransactionCreate body"] --> B["ValidateTransactionCreate"]
    B -- "Date in the future" --> E1["InvalidTransactionCreateException<br/>400"]
    B -- ok --> C{"CategoryId<br/>provided?"}
    C -- no --> F
    C -- yes --> D["CatgoriesRepository.Find"]
    D -- "not found" --> E2["TransactionCategoryNotFound<br/>400"]
    D -- found --> F["ToDataModel()"]
    F --> G["TransactionsRepository.AddAndSave"]
    G --> H["INSERT, then reload Category<br/>if CategoryId is set"]
    H --> I["ToBusinessModel()<br/>200 OK"]
```

### `GET /transactions/{id}`

```mermaid
flowchart TD
    A["GET /transactions/id"] --> B["TransactionsRepository.FindById<br/>Include Category, BankAccount"]
    B -- null --> E["TransactionNotFound<br/>404"]
    B -- row --> C["ToBusinessModel()<br/>200 OK"]
```

### `GET /transactions` (paged and filtered)

```mermaid
flowchart TD
    A["GET /transactions?pageNumber&pageSize<br/>&startDate&endDate&minAmount<br/>&maxAmount&type&search"] --> B["ValidateTransactionQuery"]
    B -- "page < 1, start > end,<br/>min > max, date in future" --> E["InvalidTransactionQueryException<br/>400, generic message"]
    B -- ok --> C["ToFilter() + ToPagination()"]
    C --> D["FindByFilter<br/>filter, skip/take, Include Category + BankAccount"]
    C --> F["CountByFilter<br/>same filter, total rows"]
    D --> G["PaginationHelper<br/>BuildResponseItemsPaged"]
    F --> G
    G --> H["ResponseItemsPaged of Transaction<br/>200 OK"]
```

The list and the count are two separate queries that share one filter.

## 4. Error mapping

`HttpResponseExceptionFilter` picks the status with a `switch` on the exception type. A new exception type that is not added to that switch is returned as a 500.

```mermaid
flowchart LR
    BE["BusinessException"] --> IDE["InvalidDataException<br/>(carries field + description)"]
    IDE --> ICE["InvalidTransactionCreateException"]
    BE --> IQE["InvalidTransactionQueryException<br/>(own Details, not InvalidDataException)"]
    BE --> TCNF["TransactionCategoryNotFound"]
    BE --> TNF["TransactionNotFound"]

    ICE --> S400["400 Bad Request<br/>specific message + errors: field and issue"]
    IQE --> S400q["400 Bad Request<br/>generic message, errors: []"]
    TCNF --> S400b["400 Bad Request<br/>specific message, errors: []"]
    TNF --> S404["404 Not Found<br/>specific message"]
    OTHER["any other exception"] --> S500["500<br/>generic message"]
```

The filter only reads `errors[]` and the specific message from `InvalidDataException`. `InvalidTransactionQueryException` has the same `Details` shape but a different base class, so an invalid `GET /transactions` query returns 400 with the generic "unable to complete your request" message and an empty `errors` array. The field that failed is lost.

Every error body is an `ErrorResponse`: `status`, `code`, `message`, `errors[]` (field and issue) and a `requestId`. The `requestId` is a new GUID per error, not a correlation id.

## 5. Data model

```mermaid
erDiagram
    CATEGORIES ||--o{ TRANSACTIONS : "has (CategoryId, nullable)"
    BANK_ACCOUNTS ||--o{ TRANSACTIONS : "has (BankAccountId, nullable)"

    TRANSACTIONS {
        long TransactionId PK
        string Name
        datetime Date
        decimal Amount
        string Description
        int Type "0 Expense, 1 Income, 2 Transfer"
        long CategoryId FK
        long BankAccountId FK
        datetime CreatedAt
        datetime UpdatedAt
    }
    CATEGORIES {
        long CategoryId PK
        string Name
        datetime CreatedAt
        datetime UpdatedAt
    }
    BANK_ACCOUNTS {
        long BankAccountId PK
        string Name
        datetime CreatedAt
        datetime UpdatedAt
    }
```

Both foreign keys use `ON DELETE SET NULL`, so deleting a category or bank account keeps its transactions. `CreatedAt` and `UpdatedAt` come from `BaseDataModel` and are generated by the database. There are no endpoints for categories or bank accounts yet.

## 6. Startup and dependency injection

Everything is registered by hand in `Expense.Logger.Api/Program.cs`.

```mermaid
flowchart TD
    S["Program.cs"] --> CS{"ConnectionStrings:<br/>ExpenseLoggerDatabase set?"}
    CS -- "no (user secrets or env var missing)" --> X["InvalidOperationException<br/>API does not start"]
    CS -- yes --> DI

    subgraph DI["Service registrations"]
        F["AddPooledDbContextFactory<br/>ExpenseLoggerDbContext (MySQL)"]
        R["Singleton: ITransactionsRepository,<br/>ICatgoriesRepository"]
        H["Singleton: ITransactionsHandler"]
        K["AddControllers + HttpResponseExceptionFilter"]
    end

    DI --> P["Pipeline: DefaultFiles, StaticAssets,<br/>OpenAPI (Development), HttpsRedirection,<br/>Authorization, Controllers,<br/>fallback to /index.html"]
```

Repositories and the handler are singletons, so none of them holds a `DbContext`. Each repository method opens a short-lived context from the pooled factory.

## 7. Local development and deployment

```mermaid
flowchart LR
    subgraph Dev["Local development"]
        direction LR
        Run["dotnet run<br/>(Expense.Logger.Api)"] -- "SpaProxy runs<br/>npm run dev" --> Vite["Vite dev server<br/>https://localhost:60193"]
        Vite -- "proxy /transactions,<br/>/weatherforecast" --> Kestrel["Kestrel<br/>https://localhost:7232"]
        Kestrel --> MySQL[("local MySQL")]
    end

    subgraph Prod["Planned: Vercel"]
        direction LR
        Portfolio["portfolio project<br/>felipelopez.vercel.app"] -- "rewrite /expense-logger/*" --> ClientProj["expense-logger project<br/>(client static build,<br/>base /expense-logger/)"]
        ClientProj -. "API calls (not hosted yet)" .-> Api["ASP.NET API<br/>needs a .NET host"]
    end
```

The client Vercel project is not created yet. Vercel cannot run the .NET API, so it has to be hosted somewhere else before the deployed client can show real data.
