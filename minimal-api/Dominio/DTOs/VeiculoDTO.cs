namespace MinimalApi.Dominio.DTOs
{
    public class VeiculoDTO
    {
        public required string Nome { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public int Ano { get; set; }
        public required string Placa { get; set; }
        public decimal Preco { get; set; }
    }
}