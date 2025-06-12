using GestionDeCompteSG.Application;
using Microsoft.AspNetCore.DataProtection.Repositories;
using GestionDeCompteSG.Persistence;
using GestionDeCompteSG.Middlewares;
using System.Reflection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IRepository, Repository>();
builder.Services.AddSingleton<IService, Service>();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gestion de Comptes API",
        Version = "v1"

    });
});

var app = builder.Build();
var csvProcessor = app.Services.GetRequiredService<IService>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();
await csvProcessor.TraiteCsvAsync(Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "account_20230228.csv"));
app.MapControllers();

app.Run();
