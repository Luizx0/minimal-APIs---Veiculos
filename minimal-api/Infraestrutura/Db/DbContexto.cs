using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
namespace MinimalApi.Infraestrutura.Db;

public class DbContexto : DbContext
{
    private readonly IConfiguration _configuration;
    public DbContexto(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public DbSet<Administrador> Administradores { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            
        

            var connectionString = _configuration.GetConnectionString("mysql").ToString();
            if(!string.IsNullOrEmpty(connectionString))
                {
                optionsBuilder.UseMySql(connectionString, 
                new MySqlServerVersion(new Version(8, 0, 26)));
            }
        }
    }


}