using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MinimalApi.Dominio.Entidades;

public class Administrador
{
    public Administrador() { }

    [SetsRequiredMembers]
    public Administrador(string nome, string email, string senha, string perfil)
    {
        Nome = nome;
        Email = email;
        Password = senha;
        Perfil = perfil;
    }

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