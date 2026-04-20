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
        public Administrador Cadastrar(Administrador administrador)
    {
        _contexto.Administradores.Add(administrador);
        _contexto.SaveChanges();
        return administrador;
    }
    
        public List<Administrador> Todos(int? pagina)
    {
        int quantidadePorPagina = 10;
        int numeroPagina = pagina ?? 1;
        return _contexto.Administradores
            .Skip((numeroPagina - 1) * quantidadePorPagina)
            .Take(quantidadePorPagina)
            .ToList();

    }
    
        public Administrador ObterPorId(int id)
    {
        return _contexto.Administradores.Find(id) ?? throw new Exception("Administrador não encontrado");
    }
    }