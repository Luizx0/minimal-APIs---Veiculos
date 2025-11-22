using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApi.Dominio.Entidades;

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

    [StringLength(30)]
    public required string Password { get; set; }   

    [StringLength(10)]
    public required string Perfil { get; set; }
}