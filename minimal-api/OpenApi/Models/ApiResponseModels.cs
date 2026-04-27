namespace MinimalApi.OpenApi.Models;

/// <summary>
/// Resposta de login bem-sucedido
/// </summary>
public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public bool Vitalicio { get; set; }
    public int? ExpiresInHours { get; set; }
}

/// <summary>
/// Resposta de erro de validação
/// </summary>
public class ValidationErrorResponse
{
    public List<string> Mensagens { get; set; } = new();
}

/// <summary>
/// Resposta genérica de criação
/// </summary>
public class CreatedResponse<T>
{
    public string Mensagem { get; set; } = string.Empty;
    public T Dados { get; set; } = default!;
}

/// <summary>
/// Resposta genérica de atualização
/// </summary>
public class UpdatedResponse<T>
{
    public string Mensagem { get; set; } = string.Empty;
    public T Dados { get; set; } = default!;
}

/// <summary>
/// Resposta de erro genérico
/// </summary>
public class ErrorResponse
{
    public string Mensagem { get; set; } = string.Empty;
    public string? Erro { get; set; }
}