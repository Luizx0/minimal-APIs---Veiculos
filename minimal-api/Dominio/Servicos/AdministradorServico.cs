using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Servicos;

    public class AdministradorServico : iAdministradorServico
    {
        private readonly DbContexto _contexto;
        public AdministradorServico(DbContexto contexto)
    {
        _contexto = contexto;
    }
        
        public Administrador? Login(LoginDTO loginDTO)
        {
            var adm = _contexto.Administradores
            .FirstOrDefault(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password);
            return adm;
    }
    }