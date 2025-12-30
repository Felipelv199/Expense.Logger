using Expense.Logger.Api.Filters;
using Expense.Logger.Business.Implementations;
using Expense.Logger.Business.Interfaces;
using Expense.Logger.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ITransactionsHandler, TransactionHandler>();

var connectionString = builder.Configuration.GetConnectionString("ExpenseLoggerDatabase")
        ?? throw new InvalidOperationException("Connection string"
        + "'ExpenseLoggerDatabase' not found.");
builder.Services.AddPooledDbContextFactory<ExpenseLoggerDbContext>(options =>
    options.UseMySQL(connectionString));

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.MapFallbackToFile("/index.html");

app.Run();
