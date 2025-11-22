using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<iAdministradorServico, AdministradorServico>();

builder.Services.AddDbContext<DbContexto>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("mysql"),
    new MySqlServerVersion(new Version(8, 0, 26)))
);

var app = builder.Build();


app.MapPost("/login", (MinimalApi.Dominio.DTOs.LoginDTO loginDTO) =>
{
    if (loginDTO.Email == "adm@exmplo.com" && loginDTO.Password == "123456")
    {
        return Results.Ok("Login com sucesso!");
    }
    else
        return Results.Unauthorized();
});

app.Run();








