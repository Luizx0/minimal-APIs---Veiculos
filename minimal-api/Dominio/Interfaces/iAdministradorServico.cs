using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Interfaces;

    public interface iAdministradorServico
{
    Administrador? Login(LoginDTO loginDTO);
}
