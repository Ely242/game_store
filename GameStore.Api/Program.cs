using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(); // Add CORS services
builder.Services.AddValidation(); // Add validation services
builder.AddGameStoreDbContext(); // Add the GameStoreDbContext to the dependency injection container

var app = builder.Build();

// Enable CORS for frontend requests
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.MapGamesEndpoints();
app.MapGenresEndpoints();

app.MigrateDatabase(); // Apply any pending migrations to the database

app.Run();
