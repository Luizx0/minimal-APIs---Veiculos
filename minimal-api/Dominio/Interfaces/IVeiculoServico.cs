using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Interfaces;

    public interface iVeiculoServico
{
    List<Veiculo> Todos(int pagina, int quantidade, string? nome = null, string? marca = null);
    Veiculo? ObterPorId(int id);
    void Cadastrar(Veiculo veiculo);
    void Atualizar(int id, Veiculo veiculo);
    void Apagar(int id, Veiculo veiculo);
}

