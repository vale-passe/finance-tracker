using System.Text.Json.Serialization;
using FinanceTracker.Api.Data;
using FinanceTracker.Api.Endpoints;
using FinanceTracker.Api.Middlewares;
using FinanceTracker.Api.Services.Implementations;
using FinanceTracker.Api.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// enum serializzati come stringhe - "Expense" invece di 1
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// registra automaticamente tutti i validatori definiti nell'assembly - scansiona
// tutte le classi che estendono AbsractValidator<T> e le registra nel container DI
// senza elencarle una per una
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddCors(options =>
    options.AddPolicy("Development", policy =>
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()));

// configurazione di EF Core con PostgreSQL
builder.Services.AddDbContext<FinanceTrackerDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
            // specifico la versione di PostgreSQL - evito che EF Core 
            // generi SQL incompatibile con la versione installata
            npgsqlOptions.SetPostgresVersion(17, 0)
    ));

// AccountService registrato prima degli altri - TransactionService e TransferService
// dipendono da esso
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<ILiabilityService, LiabilityService>();

var app = builder.Build();

// gestione globale delle eccezioni - primo middleware della catena
app.UseGlobalExceptionHandler();
app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors("Development");

app.MapAccountEndpoints();
app.MapCategoryEndpoints();
app.MapTransactionEndpoints();
app.MapTransferEndpoints();

app.Run();
