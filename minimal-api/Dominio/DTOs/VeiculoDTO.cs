namespace MinimalApi.Dominio.DTOs
{
    public class VeiculoDTO
    {
        public string Nome { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public string Placa { get; set; }
        public decimal Preco { get; set; }
    }
}