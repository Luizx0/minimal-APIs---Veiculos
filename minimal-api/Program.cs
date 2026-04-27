using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.Dominio.DTOs;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Enuns;
using Microsoft.OpenApi.Models;

#region builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<iAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<iVeiculoServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });

// JWT configuration
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? "replace_with_a_secure_key";
var jwtIssuer = jwtSection["Issuer"] ?? "minimal-api";
var jwtAudience = jwtSection["Audience"] ?? "minimal-api-users";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var auth = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(auth))
            {
                if (auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    context.Token = auth.Substring("Bearer ".Length).Trim();
                else
                    context.Token = auth; // accept token pasted without 'Bearer '
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<DbContexto>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("mysql"),
    new MySqlServerVersion(new Version(8, 0, 26)))
);

var app = builder.Build();
#endregion

app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");

string GenerateToken(Administrador adm, bool vitalicio = false)
{
    var claims = new[] {
        new Claim(ClaimTypes.NameIdentifier, adm.Id.ToString()),
        new Claim(ClaimTypes.Name, adm.Nome),
        new Claim(ClaimTypes.Email, adm.Email),
        new Claim(ClaimTypes.Role, adm.Perfil ?? "Usuario")
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        issuer: jwtIssuer,
        audience: jwtAudience,
        claims: claims,
        expires: vitalicio ? (DateTime?)null : DateTime.UtcNow.AddHours(3),
        signingCredentials: creds);

    return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
}

#region administradores

app.MapPost("/administradores/login", ([FromBody]LoginDTO loginDTO, iAdministradorServico administradorServico, HttpContext http, [FromQuery] bool vitalicio = false) =>
{
    var adm = administradorServico.Login(loginDTO);
    if (adm == null)
        return Results.Unauthorized();

    // generate token: allow vitalicio only if email matches configured owner
    var ownerEmail = builder.Configuration.GetSection("Jwt")["OwnerEmail"];
    if (vitalicio && string.Equals(adm.Email, ownerEmail, StringComparison.OrdinalIgnoreCase))
    {
        var token = GenerateToken(adm, vitalicio: true);
        return Results.Ok(new { token, vitalicio = true });
    }

    var tokenNormal = GenerateToken(adm, vitalicio: false);
    return Results.Ok(new { token = tokenNormal, expiresInHours = 3 });
}).WithTags("Administradores");

app.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, iAdministradorServico administradorServico) =>
{
    var validacao = new ErrosDeValidacao
    {
        mensagens = new List<string>()
    };
    if(administradorDTO.Email == null)
    {
        validacao.mensagens.Add("O campo 'Email' é obrigatório.");
    }
    if(administradorDTO.Nome == null)
    {
        validacao.mensagens.Add("O campo 'Nome' é obrigatório.");
    }
    if(administradorDTO.Senha == null)
    {
        validacao.mensagens.Add("O campo 'Senha' é obrigatório.");
    }
    // default perfil to Usuario when undefined
    var perfilStr = administradorDTO.Perfil.ToString() ?? Perfil.Usuario.ToString();
    if(validacao.mensagens.Count > 0)
    {
        return Results.BadRequest(validacao);
    }

    try
    {
        var administrador = new Administrador(
            administradorDTO.Nome!,
            administradorDTO.Email!,
            administradorDTO.Senha!,
            perfilStr
        );
        administradorServico.Cadastrar(administrador);
        return Results.Created($"/administradores/{administrador.Id}", new
        {
            mensagem = "Administrador cadastrado com sucesso!",
            dados = administrador
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { mensagem = "Erro ao cadastrar administrador", erro = ex.Message });
    }
}).WithTags("Administradores");

app.MapGet("/administradores", ([FromQuery] int? pagina, iAdministradorServico administradorServico, HttpContext http) =>
{
    var administradores = administradorServico.Todos(pagina);
    var isAdmin = http.User?.IsInRole("Administrador") ?? false;
    var resultado = administradores.Select(a => new {
        a.Id,
        a.Nome,
        a.Email,
        Password = isAdmin ? a.Password : new string('*', Math.Min(3, a.Password?.Length ?? 3)),
        a.Perfil
    });
    return Results.Ok(resultado);
}).WithTags("Administradores");

app.MapGet("/administradores/{id}", ([FromRoute] int id, iAdministradorServico administradorServico, HttpContext http) =>
{
    var administrador = administradorServico.ObterPorId(id);
    if (administrador == null)
        return Results.NotFound();

    var isAdmin = http.User?.IsInRole("Administrador") ?? false;
    var resultado = new {
        administrador.Id,
        administrador.Nome,
        administrador.Email,
        Password = isAdmin ? administrador.Password : new string('*', Math.Min(3, administrador.Password?.Length ?? 3)),
        administrador.Perfil
    };
    return Results.Ok(resultado);
}).WithTags("Administradores");
#endregion

#region CRUD Veículos
ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ErrosDeValidacao
    {
        mensagens = new List<string>()
    };
    if (string.IsNullOrEmpty(veiculoDTO.Nome))
    {
        validacao.mensagens.Add("O campo 'Nome' é obrigatório.");
    }
    if (string.IsNullOrEmpty(veiculoDTO.Marca))
    {
        validacao.mensagens.Add("O campo 'Marca' é obrigatório.");
    }
    if (string.IsNullOrEmpty(veiculoDTO.Modelo))
    {
        validacao.mensagens.Add("O campo 'Modelo' é obrigatório.");
    }
    if (string.IsNullOrEmpty(veiculoDTO.Placa))
    {
        validacao.mensagens.Add("O campo 'Placa' é obrigatório.");
    }
    if (veiculoDTO.Ano <= 0)
    {
        validacao.mensagens.Add("O campo 'Ano' deve ser um número positivo.");
    }
    
    return validacao;
}


app.MapPost("/veiculos", ([FromBody]VeiculoDTO veiculoDTO, iVeiculoServico veiculoServico) =>
{
    var validacao = validaDTO(veiculoDTO);
    if(validacao.mensagens.Count > 0)
    {
        return Results.BadRequest(validacao);
    }


    try
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
    catch (Exception ex)
    {
        return Results.BadRequest(new { mensagem = "Erro ao cadastrar veículo", erro = ex.Message });
    }
}
).WithTags("Veículos");

app.MapGet("/veiculos", ([FromQuery] int pagina, int quantidade, string? nome, string? marca, iVeiculoServico veiculoServico) =>
{
    var veiculos = veiculoServico.Todos(pagina, quantidade, nome, marca);
    return Results.Ok(veiculos);
}
).WithTags("Veículos");;

app.MapGet("/veiculos/{id}", ([FromRoute] int id, iVeiculoServico veiculoServico) =>
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

app.MapPut("/veiculos/{id}", ([FromRoute] int id, [FromBody] VeiculoDTO veiculoDTO, iVeiculoServico veiculoServico) =>
{
    var veiculoExistente = veiculoServico.ObterPorId(id);
    if (veiculoExistente == null)
    {
        return Results.NotFound();
    }
    var veiculoAtualizado = new Veiculo(
        nome: veiculoDTO.Nome,
        marca: veiculoDTO.Marca,
        modelo: veiculoDTO.Modelo,
        ano: veiculoDTO.Ano,
        placa: veiculoDTO.Placa,
        preco: veiculoDTO.Preco
    );
    veiculoAtualizado.Id = id; // Manter o ID
    veiculoServico.Atualizar(id, veiculoAtualizado);
    return Results.Ok(new
    {
        mensagem = "Veículo atualizado com sucesso!",
        dados = veiculoAtualizado
    });
}).WithTags("Veículos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, iVeiculoServico veiculoServico) =>
{
    var veiculo = veiculoServico.ObterPorId(id);
    if (veiculo == null)
    {
        return Results.NotFound();
    }
    veiculoServico.Apagar(id, veiculo);
    return Results.NoContent();
}).WithTags("Veículos");

#endregion

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
});
