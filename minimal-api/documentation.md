# 📖 Documentação Completa do Projeto Minimal API - Veículos

> **Versão:** 1.0.0  
> **Última Atualização:** 27/04/2026  
> **Objetivo:** Documentação completa para compreensão integral do projeto

---

## 📋 Sumário

1. [Visão Geral do Projeto](#1-visão-geral-do-projeto)
2. [Arquitetura e Estrutura](#2-arquitetura-e-estrutura)
3. [Configurações](#3-configurações)
4. [Program.cs - Rotas e Endpoints](#4-programcs---rotas-e-endpoints)
5. [Camada Domínio](#5-camada-domínio)
6. [Camada Infraestrutura](#6-camada-infraestrutura)
7. [Banco de Dados](#7-banco-de-dados)
8. [Segurança e Autenticação](#8-segurança-e-autenticação)
9. [Migrações (Migrations)](#9-migrações-migrations)
10. [Modelos OpenAPI](#10-modelos-openapi)
11. [Fluxos de Trabalho](#11-fluxos-de-trabalho)
12. [Validações](#12-validações)
13. [Problemas e Melhorias](#13-problemas-e-melhorias)

---

## 1. Visão Geral do Projeto

### 1.1 Informações Básicas

| Propriedade | Valor |
|-------------|-------|
| **Nome** | Minimal API Veículos |
| **Framework** | .NET 9.0 / ASP.NET Core |
| **Padrão** | Minimal APIs |
| **Banco de Dados** | MySQL 8.0.26 |
| **Autenticação** | JWT Bearer Token |
| **Documentação** | Swagger/OpenAPI |
| **Namespace Principal** | `minimal_api` |

### 1.2 Propósito

API RESTful para gestão de veículos e administradores, utilizando o padrão Minimal APIs do .NET. O projeto serve como estudo e demonstração das capacidades do ASP.NET Core 9.0 com autenticação JWT.

### 1.3 Tecnologias e Dependências

```xml
<!-- minimal-api.csproj -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.11" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.11" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.11" />
<PackageReference Include="Microsoft.OpenApi" Version="1.6.22" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="9.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.10.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.0.3" />
```

---

## 2. Arquitetura e Estrutura

### 2.1 Estrutura de Pastas

```
minimal-api/
├── .gitignore                          # Arquivos ignorados pelo Git
├── appsettings.json                    # Configurações produção
├── appsettings.Development.json        # Configurações desenvolvimento
├── minimal-api.csproj                  # Projeto .NET
├── Program.cs                          # Entry point + rotas
├── Properties/
│   └── launchSettings.json             # Configurações de launch
├── Dominio/                            # Camada de Domínio
│   ├── DTOs/                           # Data Transfer Objects
│   │   ├── AdministradorDTO.cs
│   │   ├── LoginDTO.cs
│   │   ├── ValidaDTO.cs
│   │   └── VeiculoDTO.cs
│   ├── Entidades/                      # Modelos de dados
│   │   ├── Administrador.cs
│   │   └── Veiculo.cs
│   ├── Enuns/                          # Enumerações
│   │   └── Perfil.cs
│   ├── Interfaces/                     # Contratos (interfaces)
│   │   ├── iAdministradorServico.cs
│   │   └── IVeiculoServico.cs
│   ├── ModelViews/                     # Modelos de visualização
│   │   ├── ErrosDeValidacao.cs
│   │   ├── Home.cs
│   │   └── PasswordModelViewl.cs
│   └── Servicos/                       # Implementações de serviços
│       ├── AdministradorServico.cs
│       └── VeiculoServico.cs
├── Infraestrutura/                     # Camada de Infraestrutura
│   └── Db/
│       └── DbContexto.cs               # DbContext EF Core
├── Migrations/                         # Migrações EF Core
│   ├── 20251118233827_AdministradorMigration.cs
│   ├── 20251118233827_AdministradorMigration.Designer.cs
│   ├── 20251119195936_SeedAdministrador.cs
│   ├── 20251119195936_SeedAdministrador.Designer.cs
│   ├── 20260209081825_VeiculosMigration.cs
│   ├── 20260209081825_VeiculosMigration.Designer.cs
│   └── DbContextoModelSnapshot.cs
└── OpenApi/                            # Modelos para documentação
    └── Models/
        ├── ApiResponseModels.cs
        └── ResponseModels.cs
```

### 2.2 Padrão de Arquitetura

O projeto segue uma arquitetura simplificada de **Camadas**:

```
┌─────────────────────────────────────┐
│         Program.cs                  │  ← Presentation Layer
│    (Minimal APIs + Config)          │
└─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────┐
│         Dominio/Servicos            │  ← Business Layer
│    (Lógica de negócio)              │
└─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────┐
│         Dominio/Interfaces          │  ← Contract Layer
│    (Contratos dos serviços)         │
└─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────┐
│         Infraestrutura/Db          │  ← Data Layer
│    (DbContext + Repositories)       │
└─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────┐
│            MySQL                    │  ← Database
└─────────────────────────────────────┘
```

---

## 3. Configurações

### 3.1 appsettings.json

**Caminho:** `minimal-api/appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "mysql": "Server=localhost;Database=minimal_api;Uid=root;Pwd=Lu!z1603;"
  },
  "Jwt": {
    "Key": "ALONG_RANDOM_SECRET_KEY_CHANGE_ME_!",
    "Issuer": "minimal-api",
    "Audience": "minimal-api-users",
    "OwnerEmail": "adm@exemplo.com"
  }
}
```

**Configurações:**
- `ConnectionStrings:mysql` - String de conexão do MySQL
- `Jwt:Key` - Chave secreta para assinatura dos tokens JWT
- `Jwt:Issuer` - Emissor do token
- `Jwt:Audience` - Audiência do token
- `Jwt:OwnerEmail` - Email do proprietário (acesso a tokens vitalícios)

### 3.2 appsettings.Development.json

**Caminho:** `minimal-api/appsettings.Development.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 3.3 Properties/launchSettings.json

**Caminho:** `minimal-api/Properties/launchSettings.json`

Configurações de inicialização:
- **HTTP:** `http://localhost:5045`
- **HTTPS:** `https://localhost:7193`

---

## 4. Program.cs - Rotas e Endpoints

**Caminho:** `minimal-api/Program.cs`

O `Program.cs` é o arquivo principal que configura:
1. Injeção de dependências
2. Serviços (Swagger, JWT, Database)
3. Todas as rotas da API

### 4.1 Configuração de Serviços (#region builder)

```csharp
// Injeção de dependências
builder.Services.AddScoped<iAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<iVeiculoServico, VeiculoServico>();

// Swagger com segurança JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => { /* configuração JWT */ });

// Autenticação JWT
builder.Services.AddAuthentication(options => { /* JWT config */ })
.AddJwtBearer(options => { /* token validation */ });

builder.Services.AddAuthorization();

// Database Context
builder.Services.AddDbContext<DbContexto>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("mysql"),
    new MySqlServerVersion(new Version(8, 0, 26))));
```

### 4.2 Endpoints da API

#### 4.2.1 Home

| Método | Rota | Descrição | Autenticação |
|--------|------|-----------|--------------|
| GET | `/` | Retorna mensagem de boas-vindas | Não |

**Response:**
```json
{
  "Message": "Bem-vindo à API de Veículos - minimalAPI",
  "Doc": "/swagger"
}
```

---

#### 4.2.2 Administradores

| Método | Rota | Descrição | Autenticação |
|--------|------|-----------|--------------|
| POST | `/administradores/login` | Realizar login | Não |
| POST | `/administradores` | Cadastrar novo administrador | Não |
| GET | `/administradores` | Listar administradores (paginado) | Sim |
| GET | `/administradores/{id}` | Obter administrador por ID | Sim |

##### POST /administradores/login

**Request:**
```json
{
  "email": "adm@exemplo.com",
  "password": "123456"
}
```

**Query Parameters:**
- `vitalicio` (bool, opcional): Gera token sem expiração (apenas para OwnerEmail configurado)

**Response (Sucesso):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresInHours": 3
}
```
**Response (Token vitalício):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "vitalicio": true
}
```
**Response (Erro):** `401 Unauthorized`

---

##### POST /administradores

**Request:**
```json
{
  "nome": "Novo Admin",
  "email": "novo@exemplo.com",
  "senha": "senha123",
  "perfil": 0
}
```

**Validações (no Program.cs):**
- Email: Obrigatório
- Nome: Obrigatório
- Senha: Obrigatório
- Perfil: Padrão = Usuario

**Response (201 Created):**
```json
{
  "mensagem": "Administrador cadastrado com sucesso!",
  "dados": { "id": 2, "nome": "...", "email": "...", "password": "...", "perfil": "Usuario" }
}
```

**Response (400 Bad Request):**
```json
{
  "mensagens": [ "O campo 'Email' é obrigatório.", "O campo 'Nome' é obrigatório.", "O campo 'Senha' é obrigatório." ]
}
```

---

##### GET /administradores

**Query Parameters:**
- `pagina` (int, opcional): Número da página (padrão = 1)

**Response:**
```json
[
  {
    "id": 1,
    "nome": "Administrador",
    "email": "adm@exemplo.com",
    "password": "***",
    "perfil": "Administrador"
  }
]
```

> **Nota:** A senha é mascarada (`***`) para usuários não-admin.

---

##### GET /administradores/{id}

**Response (200 OK):**
```json
{
  "id": 1,
  "nome": "Administrador",
  "email": "adm@exemplo.com",
  "password": "***",
  "perfil": "Administrador"
}
```

**Response (404 Not Found):** Se o ID não existir

---

#### 4.2.3 Veículos

| Método | Rota | Descrição | Autenticação |
|--------|------|-----------|--------------|
| POST | `/veiculos` | Criar novo veículo | Não |
| GET | `/veiculos` | Listar veículos (com filtros) | Não |
| GET | `/veiculos/{id}` | Obter veículo por ID | Não |
| PUT | `/veiculos/{id}` | Atualizar veículo | Não |
| DELETE | `/veiculos/{id}` | Deletar veículo | Não |

##### POST /veiculos

**Request:**
```json
{
  "nome": "Gol",
  "marca": "Volkswagen",
  "modelo": "G6",
  "ano": 2023,
  "placa": "ABC1234",
  "preco": 45000.00
}
```

**Validações (função validaDTO no Program.cs):**
- Nome: Obrigatório
- Marca: Obrigatório
- Modelo: Obrigatório
- Placa: Obrigatório
- Ano: Deve ser positivo

**Response (201 Created):**
```json
{
  "mensagem": "Veículo cadastrado com sucesso!",
  "dados": { "id": 1, "nome": "Gol", "marca": "Volkswagen", ... }
}
```

**Response (400 Bad Request):**
```json
{
  "mensagens": [ "O campo 'Nome' é obrigatório.", "O campo 'Marca' é obrigatório.", ... ]
}
```

---

##### GET /veiculos

**Query Parameters:**
- `pagina` (int, obrigatório): Número da página (começa em 0)
- `quantidade` (int, obrigatório): Itens por página
- `nome` (string, opcional): Filtro por nome (contém)
- `marca` (string, opcional): Filtro por marca (contém)

**Response:**
```json
[
  { "id": 1, "nome": "Gol", "marca": "Volkswagen", "modelo": "G6", "ano": 2023, "placa": "ABC1234", "preco": 45000.00 }
]
```

---

##### GET /veiculos/{id}

**Response (200 OK):**
```json
{ "id": 1, "nome": "Gol", "marca": "Volkswagen", ... }
```

**Response (404 Not Found):** Se o ID não existir

---

##### PUT /veiculos/{id}

**Request:** Mesmo esquema do POST

**Response (200 OK):**
```json
{
  "mensagem": "Veículo atualizado com sucesso!",
  "dados": { ... }
}
```

**Response (404 Not Found):** Se o ID não existir

---

##### DELETE /veiculos/{id}

**Response:** `204 No Content`

**Response (404 Not Found):** Se o ID não existir

---

## 5. Camada Domínio

### 5.1 Entidades

#### 5.1.1 Administrador

**Caminho:** `minimal-api/Dominio/Entidades/Administrador.cs`

```csharp
public class Administrador
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public required string Nome { get; set; }

    [Required]
    [StringLength(100)]
    public required string Email { get; set; }

    [Required]
    [StringLength(30)]
    public required string Password { get; set; }

    [Required]
    [StringLength(10)]
    public required string Perfil { get; set; }
}
```

**Campos:**
| Campo | Tipo | Tamanho | Obrigatório | Descrição |
|-------|------|----------|-------------|------------|
| Id | int | - | Sim | Chave primária (auto-incremento) |
| Nome | string | 100 | Sim | Nome do administrador |
| Email | string | 100 | Sim | Email único do administrador |
| Password | string | 30 | Sim | Senha (armazenada em texto plano!) |
| Perfil | string | 10 | Sim | Perfil: "Administrador" ou "Usuario" |

---

#### 5.1.2 Veiculo

**Caminho:** `minimal-api/Dominio/Entidades/Veiculo.cs`

```csharp
public class Veiculo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Nome { get; set; }

    [Required]
    [StringLength(100)]
    public string Marca { get; set; }

    [Required]
    public int Ano { get; set; }

    [Required]
    [StringLength(10)]
    public string Placa { get; set; }
    
    public string Modelo { get; internal set; }
    public decimal Preco { get; internal set; }
}
```

**Campos:**
| Campo | Tipo | Tamanho | Obrigatório | Descrição |
|-------|------|----------|-------------|------------|
| Id | int | - | Sim | Chave primária (auto-incremento) |
| Nome | string | 150 | Sim | Nome do veículo |
| Marca | string | 100 | Sim | Marca do veículo |
| Modelo | string | - | Não | Modelo do veículo (setter interno) |
| Ano | int | - | Sim | Ano de fabricação |
| Placa | string | 10 | Sim | Placa do veículo |
| Preco | decimal | - | Não | Preço do veículo (setter interno) |

---

### 5.2 DTOs (Data Transfer Objects)

#### 5.2.1 AdministradorDTO

**Caminho:** `minimal-api/Dominio/DTOs/AdministradorDTO.cs`

```csharp
public class AdministradorDTO
{
    public string Nome { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Senha { get; set; } = default!;
    public Perfil Perfil { get; set; } = default!;
}
```

**Uso:** Transferência de dados para criação de administradores

---

#### 5.2.2 LoginDTO

**Caminho:** `minimal-api/Dominio/DTOs/LoginDTO.cs`

```csharp
public class LoginDTO
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
```

**Uso:** Autenticação de administradores

---

#### 5.2.3 VeiculoDTO

**Caminho:** `minimal-api/Dominio/DTOs/VeiculoDTO.cs`

```csharp
public class VeiculoDTO
{
    public required string Nome { get; set; }
    public required string Marca { get; set; }
    public required string Modelo { get; set; }
    public int Ano { get; set; }
    public required string Placa { get; set; }
    public decimal Preco { get; set; }
}
```

**Uso:** Transferência de dados para criação/atualização de veículos

---

#### 5.2.4 ValidaDTO

**Caminho:** `minimal-api/Dominio/DTOs/ValidaDTO.cs`

**Status:** Arquivo vazio (não utilizado)

---

### 5.3 Enums

#### 5.3.1 Perfil

**Caminho:** `minimal-api/Dominio/Enuns/Perfil.cs`

```csharp
public enum Perfil
{
    Administrador = 0,
    Usuario = 1
}
```

**Uso:** Definir perfil do administrador

---

### 5.4 Interfaces de Serviços

#### 5.4.1 iAdministradorServico

**Caminho:** `minimal-api/Dominio/Interfaces/iAdministradorServico.cs`

```csharp
public interface iAdministradorServico
{
    Administrador? Login(LoginDTO loginDTO);
    Administrador Cadastrar(Administrador administrador);
    List<Administrador> Todos(int? pagina);
    Administrador ObterPorId(int id);
}
```

**Métodos:**
| Método | Retorno | Descrição |
|--------|---------|-----------|
| Login | Administrador? | Busca administrador por email e senha |
| Cadastrar | Administrador | Cadastra novo administrador |
| Todos | List<Administrador> | Lista administradores com paginação |
| ObterPorId | Administrador | Busca administrador por ID |

---

#### 5.4.2 IVeiculoServico

**Caminho:** `minimal-api/Dominio/Interfaces/IVeiculoServico.cs`

```csharp
public interface iVeiculoServico
{
    List<Veiculo> Todos(int pagina, int quantidade, string? nome = null, string? marca = null);
    Veiculo? ObterPorId(int id);
    void Cadastrar(Veiculo veiculo);
    void Atualizar(int id, Veiculo veiculo);
    void Apagar(int id, Veiculo veiculo);
}
```

**Métodos:**
| Método | Retorno | Descrição |
|--------|---------|-----------|
| Todos | List<Veiculo> | Lista veículos com paginação e filtros |
| ObterPorId | Veiculo? | Busca veículo por ID |
| Cadastrar | void | Cadastra novo veículo |
| Atualizar | void | Atualiza veículo existente |
| Apagar | void | Remove veículo |

---

### 5.5 Implementações de Serviços

#### 5.5.1 AdministradorServico

**Caminho:** `minimal-api/Dominio/Servicos/AdministradorServico.cs`

```csharp
public class AdministradorServico : iAdministradorServico
{
    private readonly DbContexto _contexto;
    
    public AdministradorServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public Administrador? Login(LoginDTO loginDTO)
    {
        return _contexto.Administradores
            .FirstOrDefault(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password);
    }

    public Administrador Cadastrar(Administrador administrador)
    {
        _contexto.Administradores.Add(administrador);
        _contexto.SaveChanges();
        return administrador;
    }

    public List<Administrador> Todos(int? pagina)
    {
        int quantidadePorPagina = 10;
        int numeroPagina = pagina ?? 1;
        return _contexto.Administradores
            .Skip((numeroPagina - 1) * quantidadePorPagina)
            .Take(quantidadePorPagina)
            .ToList();
    }

    public Administrador ObterPorId(int id)
    {
        return _contexto.Administradores.Find(id) 
            ?? throw new Exception("Administrador não encontrado");
    }
}
```

---

#### 5.5.2 VeiculoServico

**Caminho:** `minimal-api/Dominio/Servicos/VeiculoServico.cs`

```csharp
public class VeiculoServico : iVeiculoServico
{
    private readonly DbContexto _contexto;

    public VeiculoServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public List<Veiculo> Todos(int pagina, int quantidade, string? nome = null, string? marca = null)
    {
        IQueryable<Veiculo> query = _contexto.Veiculos;
        
        if (!string.IsNullOrEmpty(nome))
            query = query.Where(v => v.Nome.ToLower().Contains(nome.ToLower()));
        
        if (!string.IsNullOrEmpty(marca))
            query = query.Where(v => v.Marca.ToLower().Contains(marca.ToLower()));
        
        return query.Skip(pagina * quantidade).Take(quantidade).ToList();
    }

    public Veiculo? ObterPorId(int id)
    {
        return _contexto.Veiculos.FirstOrDefault(v => v.Id == id);
    }

    public void Cadastrar(Veiculo veiculo)
    {
        _contexto.Veiculos.Add(veiculo);
        _contexto.SaveChanges();
    }

    public void Atualizar(int id, Veiculo veiculo)
    {
        _contexto.Veiculos.Update(veiculo);
        _contexto.SaveChanges();
    }

    public void Apagar(int id, Veiculo veiculo)
    {
        _contexto.Veiculos.Remove(veiculo);
        _contexto.SaveChanges();
    }
}
```

---

### 5.6 Model Views

#### 5.6.1 ErrosDeValidacao

**Caminho:** `minimal-api/Dominio/ModelViews/ErrosDeValidacao.cs`

```csharp
public struct ErrosDeValidacao
{
    public List<string> mensagens { get; set; }
}
```

**Uso:** Retornar erros de validação em respostas 400

---

#### 5.6.2 Home

**Caminho:** `minimal-api/Dominio/ModelViews/Home.cs`

```csharp
public struct Home
{
    public string? Message { get => "Bem-vindo à API de Veículos - minimalAPI"; }
    public string? Doc { get => "/swagger"; }
}
```

**Uso:** Resposta do endpoint `/`

---

#### 5.6.3 PasswordModelViewl

**Caminho:** `minimal-api/Dominio/ModelViews/PasswordModelViewl.cs`

```csharp
public record PasswordModelView
{
    public string? SenhaAtual { get; set; }
    public string? NovaSenha { get; set; }

    public static string Display(string? senha, bool isAdmin)
    {
        if (isAdmin) return senha ?? string.Empty;
        return new string('*', Math.Min(3, senha?.Length ?? 3));
    }
}
```

**Status:** Não utilizado atualmente (sem endpoint para mudança de senha)

---

## 6. Camada Infraestrutura

### 6.1 DbContexto

**Caminho:** `minimal-api/Infraestrutura/Db/DbContexto.cs`

```csharp
public class DbContexto : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Administrador> Administradores { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>().HasData(
            new Administrador
            {
                Id = 1,
                Nome = "Administrador",
                Email = "adm@exemplo.com",
                Password = "123456",
                Perfil = "Administrador"
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("mysql");
            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseMySql(connectionString,
                    new MySqlServerVersion(new Version(8, 0, 26)));
            }
        }
    }
}
```

**Características:**
- DbSets: `Administradores`, `Veiculos`
- Seeding: 1 administrador padrão
- Banco: MySQL 8.0.26

---

## 7. Banco de Dados

### 7.1 Tabela: Administradores

```sql
CREATE TABLE Administradores (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Password VARCHAR(30) NOT NULL,
    Perfil VARCHAR(10) NOT NULL,
    CHARSET utf8mb4
);

-- Seed padrão:
INSERT INTO Administradores VALUES 
(1, 'Administrador', 'adm@exemplo.com', '123456', 'Administrador');
```

### 7.2 Tabela: Veiculos

```sql
CREATE TABLE Veiculos (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(150) NOT NULL,
    Marca VARCHAR(100) NOT NULL,
    Ano INT NOT NULL,
    Placa VARCHAR(10) NOT NULL,
    CHARSET utf8mb4
);
```

> **Nota:** As colunas `Modelo` e `Preco` existem na entidade C# mas não estão no banco de dados conforme as migrations atuais.

---

## 8. Segurança e Autenticação

### 8.1 JWT Bearer Token

#### 8.1.1 Geração de Token

**Local:** `Program.cs` - função `GenerateToken`

```csharp
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
```

**Características:**
- **Claims incluídos:** ID, Nome, Email, Perfil
- **Algoritmo:** HMAC SHA256
- **Expiração:** 3 horas (padrão) ou null (vitalício)
- **Validação:** Issuer, Audience, SigningKey

#### 8.1.2 Configuração JWT

```csharp
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
    // Suporta token com ou sem prefixo "Bearer "
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context => { /* ... */ }
    };
});
```

#### 8.1.3 Configuração Swagger JWT

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
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
});
```

---

## 9. Migrações (Migrations)

### 9.1 Histórico de Migrações

| Data | Migration | Descrição |
|------|-----------|-----------|
| 18/11/2025 | 20251118233827 | Criar tabela Administradores |
| 19/11/2025 | 20251119195936 | Seed administrador padrão |
| 09/02/2026 | 20260209081825 | Criar tabela Veículos |

### 9.2 20251118233827_AdministradorMigration

**Caminho:** `minimal-api/Migrations/20251118233827_AdministradorMigration.cs`

**Objetivo:** Criar tabela Administradores

```csharp
migrationBuilder.CreateTable(
    name: "Administradores",
    columns: table => new
    {
        Id = table.Column<int>(type: "int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
        Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
        Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
        Password = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
        Perfil = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Administradores", x => x.Id);
    });
```

---

### 9.3 20251119195936_SeedAdministrador

**Caminho:** `minimal-api/Migrations/20251119195936_SeedAdministrador.cs`

**Objetivo:** Inserir administrador padrão

```csharp
migrationBuilder.InsertData(
    table: "Administradores",
    columns: new[] { "Id", "Nome", "Email", "Password", "Perfil" },
    values: new object[] { 1, "Administrador", "Adm1@exemplo.com", "123456", "Adm" });
```

---

### 9.4 20260209081825_VeiculosMigration

**Caminho:** `minimal-api/Migrations/20260209081825_VeiculosMigration.cs`

**Objetivo:** Criar tabela Veículos

```csharp
migrationBuilder.CreateTable(
    name: "Veiculos",
    columns: table => new
    {
        Id = table.Column<int>(type: "int", nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
        Nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
        Marca = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
        Ano = table.Column<int>(type: "int", nullable: false),
        Placa = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Veiculos", x => x.Id);
    });
```

---

## 10. Modelos OpenAPI

### 10.1 ApiResponseModels

**Caminho:** `minimal-api/OpenApi/Models/ApiResponseModels.cs`

```csharp
namespace MinimalApi.OpenApi.Models;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public bool Vitalicio { get; set; }
    public int? ExpiresInHours { get; set; }
}

public class ValidationErrorResponse
{
    public List<string> Mensagens { get; set; } = new();
}

public class CreatedResponse<T>
{
    public string Mensagem { get; set; } = string.Empty;
    public T Dados { get; set; } = default!;
}

public class UpdatedResponse<T>
{
    public string Mensagem { get; set; } = string.Empty;
    public T Dados { get; set; } = default!;
}

public class ErrorResponse
{
    public string Mensagem { get; set; } = string.Empty;
    public string? Erro { get; set; }
}
```

---

### 10.2 ResponseModels

**Caminho:** `minimal-api/OpenApi/Models/ResponseModels.cs`

```csharp
namespace MinimalApi.OpenApi.Models;

public class ResponseModel<T>
{
    public T? Dados { get; set; }
    public string? Mensagem { get; set; }
    public bool Sucesso => true;
}

public class ErroResponse
{
    public string Mensagem { get; set; } = string.Empty;
    public string? Erro { get; set; }
    public List<string>? Erros { get; set; }
}

public class PaginacaoModel
{
    public int Pagina { get; set; }
    public int Quantidade { get; set; }
    public int Total { get; set; }
}
```

---

## 11. Fluxos de Trabalho

### 11.1 Fluxo de Login

```
┌──────────────┐     ┌─────────────────┐     ┌──────────────────┐
│   Cliente    │────▶│ POST /login     │────▶│ Administrador    │
│              │     │ {email, password}│     │ Servico.Login()  │
└──────────────┘     └─────────────────┘     └──────────────────┘
                                                        │
                                                        ▼
                                               ┌──────────────────┐
                                               │ Busca no DB por  │
                                               │ Email + Password │
                                               └──────────────────┘
                                                        │
                           ┌────────────────────────────┴─────┐
                           ▼                                      ▼
                    ┌────────────────┐                    ┌────────────────┐
                    │  Encontrado   │                    │   Não achou   │
                    └────────────────┘                    └────────────────┘
                           │                                      │
                           ▼                                      ▼
                    ┌────────────────┐                    ┌────────────────┐
                    │ Gera JWT Token │                    │ 401 Unauthorized│
                    │ (3h ou vitalício)│                   └────────────────┘
                    └────────────────┘
                           │
                           ▼
                    ┌────────────────┐
                    │ { token,       │
                    │   expiresIn } │
                    └────────────────┘
```

### 11.2 Fluxo de Criação de Veículo

```
┌──────────────┐     ┌─────────────────┐     ┌──────────────────┐
│   Cliente    │────▶│ POST /veiculos  │────▶│ validaDTO()     │
│              │     │ {veiculoDTO}    │     │ (valida campos) │
└──────────────┘     └─────────────────┘     └──────────────────┘
                                                        │
                           ┌────────────────────────────┴─────┐
                           ▼                                      ▼
                    ┌────────────────┐                    ┌────────────────┐
                    │  Válido        │                    │   Inválido     │
                    └────────────────┘                    └────────────────┘
                           │                                      │
                           ▼                                      ▼
                    ┌────────────────┐                    ┌────────────────┐
                    │ Cria Veiculo   │                    │ 400 BadRequest│
                    │ (entidade)     │                    │ (erros)        │
                    └────────────────┘                    └────────────────┘
                           │
                           ▼
                    ┌────────────────┐
                    │ VeiculoServico │
                    │ .Cadastrar()   │
                    └────────────────┘
                           │
                           ▼
                    ┌────────────────┐
                    │ 201 Created    │
                    │ {mensagem,     │
                    │   dados}       │
                    └────────────────┘
```

---

## 12. Validações

### 12.1 Validações de Administrador

**Local:** `Program.cs` - endpoint `POST /administradores`

| Campo | Obrigatório | Tipo | Validação |
|-------|-------------|------|-----------|
| Email | Sim | string | Não nulo/vazio |
| Nome | Sim | string | Não nulo/vazio |
| Senha | Sim | string | Não nulo/vazio |
| Perfil | Não | Perfil | Padrão = Usuario |

**Código:**
```csharp
if(administradorDTO.Email == null)
    validacao.mensagens.Add("O campo 'Email' é obrigatório.");
if(administradorDTO.Nome == null)
    validacao.mensagens.Add("O campo 'Nome' é obrigatório.");
if(administradorDTO.Senha == null)
    validacao.mensagens.Add("O campo 'Senha' é obrigatório.");
```

---

### 12.2 Validações de Veículo

**Local:** `Program.cs` - função `validaDTO()`

| Campo | Obrigatório | Tipo | Validação |
|-------|-------------|------|-----------|
| Nome | Sim | string | Não vazio |
| Marca | Sim | string | Não vazio |
| Modelo | Sim | string | Não vazio |
| Placa | Sim | string | Não vazio |
| Ano | Sim | int | > 0 |

**Código:**
```csharp
if (string.IsNullOrEmpty(veiculoDTO.Nome))
    validacao.mensagens.Add("O campo 'Nome' é obrigatório.");
if (string.IsNullOrEmpty(veiculoDTO.Marca))
    validacao.mensagens.Add("O campo 'Marca' é obrigatório.");
if (string.IsNullOrEmpty(veiculoDTO.Modelo))
    validacao.mensagens.Add("O campo 'Modelo' é obrigatório.");
if (string.IsNullOrEmpty(veiculoDTO.Placa))
    validacao.mensagens.Add("O campo 'Placa' é obrigatório.");
if (veiculoDTO.Ano <= 0)
    validacao.mensagens.Add("O campo 'Ano' deve ser um número positivo.");
```

---

## 13. Problemas e Melhorias

### 13.1 Problemas de Segurança

| # | Problema | Severidade | Solução |
|---|----------|------------|---------|
| 1 | Senhas em texto plano | CRÍTICO | Implementar hash (BCrypt, Argon2) |
| 2 | JWT Key em appsettings | ALTO | Mover para variáveis de ambiente |
| 3 | Sem HTTPS obrigatório | ALTO | Configurar redirect HTTPS |
| 4 | Sem rate limiting | MÉDIO | Adicionar middleware de rate limit |
| 5 | Sem proteção brute force | MÉDIO | Implementar lockout após tentativas |

### 13.2 Problemas de Banco de Dados

| # | Problema | Severidade | Solução |
|---|----------|------------|---------|
| 1 | Colunas Modelo/Preco não existem no DB | MÉDIO | Criar migration para adicionar |
| 2 | Sem índices nas colunas de busca | BAIXO | Adicionar índices para nome/marca |

### 13.3 Problemas de Código

| # | Problema | Severidade | Solução |
|---|----------|------------|---------|
| 1 | Validações duplicadas no Program.cs | MÉDIO | Criar classes de validação reutilizáveis |
| 2 | ValidaDTO vazio | BAIXO | Implementar ou remover |
| 3 | PasswordModelView não utilizado | BAIXO | Implementar endpoint de mudança de senha |
| 4 | Sem tratamento de exceções global | MÉDIO | Adicionar middleware de exception handling |

### 13.4 Melhorias Sugeridas

1. **Autenticação:**
   - Implementar refresh tokens
   - Adicionar 2FA (Two-Factor Authentication)
   - Implementar logout (token revocation)

2. **Validação:**
   - Usar FluentValidation para validações reutilizáveis
   - Adicionar Data Annotations nas entidades

3. **Documentação:**
   - Adicionar XML Comments nos endpoints
   - Implementar exemplos no Swagger

4. **性能:**
   - Adicionar caching (Redis)
   - Implementar paginação com total de registros

5. **Testes:**
   - Implementar testes unitários (xUnit)
   - Implementar testes de integração

---

## 📊 Resumo Técnico

| Aspecto | Detalhes |
|---------|----------|
| **Framework** | ASP.NET Core 9.0 |
| **Padrão de API** | Minimal APIs |
| **ORM** | Entity Framework Core 9.0.11 |
| **Banco de Dados** | MySQL 8.0.26 |
| **Autenticação** | JWT Bearer |
| **Documentação** | Swagger/OpenAPI |
| **Portas** | HTTP: 5045, HTTPS: 7193 |
| **Total de Rotas** | 9 endpoints |
| **DTOs** | 4 classes |
| **Entidades** | 2 tabelas |
| **Serviços** | 2 implementações |
| **Migrations** | 3 migrações |

---

## 🔄 Como Atualizar Este Documento

Quando fazer alterações no projeto, atualize este documento seguindo estas diretrizes:

### Para novas rotas:
1. Adicionar na seção [4.2](#42-endpoints-da-api)
2. Incluir método HTTP, rota, descrição e autenticação
3. Adicionar exemplos de request/response
4. Documentar validações na seção [12](#12-validações)

### Para novas entidades:
1. Adicionar em [5.1](#51-entidades)
2. Incluir campos, tipos e restrições
3. Atualizar schema do banco em [7](#7-banco-de-dados)

### Para novas dependências:
1. Atualizar em [1.3](#13-tecnologias-e-dependências)
2. Documentar propósito da dependência

### Para correções de segurança:
1. Atualizar seção [8](#8-segurança-e-autenticação)
2. Adicionar novo problema em [13.1](#131-problemas-de-segurança)

---

**Documento criado em: 27/04/2026**  
**Última atualização: 27/04/2026**  
**Projeto: Minimal API Veículos - .NET 9.0**