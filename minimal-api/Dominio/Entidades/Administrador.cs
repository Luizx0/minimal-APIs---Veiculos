namespace MinimalApi.Dominio.Entidades;

public class Administrador
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }   
    public string Perfil { get; set; }
}