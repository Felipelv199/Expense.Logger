# How to manage database migrations?

To manage database migrations effectively, you can follow these steps:

## To create a new migration:

After making changes to your data model or models, you can create a new migration using the following command in your terminal:

```powershell
cd ./Expense.Logger.Api ; dotnet ef migrations add InitialCreate --project ..\Expense.Logger.Data
```

This command will generate a new migration file to the Migrations folder in the specified project directory (`Expense.Logger.Data` in this case).

## To apply migrations to the database:

After creating the migration, you can apply it to your database using the following command:

```powershell
cd ./Expense.Logger.Api ; dotnet ef database update --project ..\Expense.Logger.Data
```

This command will update your database schema to match the changes defined in your migration files.

## To remove migrations from the database:

You can remove the migration changes from your database using the following command:

```powershell
cd ./Expense.Logger.Api ; dotnet ef database drop
```

This command will remove your database tables and schemas changes.