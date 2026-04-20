using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.Dominio.DTOs;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Dominio.ModelViews;

#region builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<iAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<iVeiculoServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("mysql"),
    new MySqlServerVersion(new Version(8, 0, 26)))
);

var app = builder.Build();
#endregion

app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");

#region administradores

app.MapPost("/administradores/login", ([FromBody]LoginDTO loginDTO, iAdministradorServico administradorServico) =>
{
    if (administradorServico.Login(loginDTO) != null)
    {
        return Results.Ok("Login com sucesso!");
    }
    else
        return Results.Unauthorized();
}).WithTags("Administradores");
#endregion

#region CRUD Veículos
app.MapPost("/veiculos", ([FromBody]VeiculoDTO veiculoDTO, iVeiculoServico veiculoServico) =>
{
    var veiculo = new Veiculo(
        nome: veiculoDTO.Nome,
        marca: veiculoDTO.Marca,
        modelo: veiculoDTO.Modelo,
        ano: veiculoDTO.Ano,
        placa: veiculoDTO.Placa,
        preco: veiculoDTO.Preco
    );
    veiculoServico.Cadastrar(veiculo);
    return Results.Created($"/veiculos/{veiculo.Id}", new
    {
        mensagem = "Veículo cadastrado com sucesso!",
        dados = veiculo
    });
}
).WithTags("Veículos");

app.MapGet("/veiculos", (int pagina, int quantidade, string? nome, string? marca, iVeiculoServico veiculoServico) =>
{
    var veiculos = veiculoServico.Todos(pagina, quantidade, nome, marca);
    if (veiculos != null && veiculos.Count > 0)
    {
        return Results.Ok(veiculos);
    }
    else
        return Results.NotFound();
}
).WithTags("Veículos");;

app.MapGet("/veiculos/{id}", (int id, iVeiculoServico veiculoServico) =>
{
    var veiculo = veiculoServico.ObterPorId(id);
    if (veiculo != null)
    {
        return Results.Ok(veiculo);
    }
    else
        return Results.NotFound();
}
).WithTags("Veículos");;

#endregion

app.UseSwagger();
app.UseSwaggerUI();


app.Run();