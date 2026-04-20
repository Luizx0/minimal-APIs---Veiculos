using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApi.Dominio.Entidades;

public class Veiculo
{
    public Veiculo(string nome, string marca, string modelo, int ano, string placa, decimal preco)
    {
        Nome = nome;
        Marca = marca;
        Modelo = modelo;
        Ano = ano;
        Placa = placa;
        Preco = preco;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public required string Nome { get; set; }

    [Required]
    [StringLength(100)]
    public required string Marca { get; set; }

    [Required]
    public required int Ano { get; set; }   

    [Required]
    [StringLength(10)]
    public required string Placa { get; set; }
    public string Modelo { get; internal set; }
    public decimal Preco { get; internal set; }
}