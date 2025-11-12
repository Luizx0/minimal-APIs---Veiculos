var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

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

