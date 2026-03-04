using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.Dominio.DTOs;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Dominio.ModelViews;

#region Builder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("mysql"),
    new MySqlServerVersion(new Version(8, 0, 26)))
);

var app = builder.Build();
#endregion

# region Home
app.MapGet("/", () => new Home());
# endregion

#region Administrador
app.MapPost("/administrador/login", ([FromBody]LoginDTO loginDTO, IAdministradorServico administradorServico) =>
{
    if (administradorServico.Login(loginDTO) != null)
    {
        return Results.Ok("Login com sucesso!");
    }
    else
        return Results.Unauthorized();
});
#endregion

#region Veiculos
app.MapPost("/veiculos", ([FromBody]VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
{
    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano,
        Placa = veiculoDTO.Placa
    };
    veiculoServico.Cadastrar(veiculo);

    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
});
#endregion

#region App>
app.UseSwagger();
app.UseSwaggerUI();


app.Run();
#endregion