namespace MinimalApi.OpenApi.Models;

/// <summary>
/// Modelo de resposta padrão para operações bem-sucedidas
/// </summary>
public class ResponseModel<T>
{
    public T? Dados { get; set; }
    public string? Mensagem { get; set; }
    public bool Sucesso => true;
}

/// <summary>
/// Modelo de resposta de erro
/// </summary>
public class ErroResponse
{
    public string Mensagem { get; set; } = string.Empty;
    public string? Erro { get; set; }
    public List<string>? Erros { get; set; }
}

/// <summary>
/// Modelo de paginação
/// </summary>
public class PaginacaoModel
{
    public int Pagina { get; set; }
    public int Quantidade { get; set; }
    public int Total { get; set; }
}