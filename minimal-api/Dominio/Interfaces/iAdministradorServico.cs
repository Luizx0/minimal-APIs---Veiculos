using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Interfaces;

    public interface IAdministradorServico
{
    Administrador? Login(LoginDTO loginDTO);
    Administrador Cadastrar(Administrador administrador);
    List<Administrador> Todos(int? pagina);
    Administrador ObterPorId(int id);
}
