namespace MinimalApi.Dominio.ModelViews;

public record PasswordModelView
{
    public string? SenhaAtual { get; set; }
    public string? NovaSenha { get; set; }

    public static string Display(string? senha, bool isAdmin)
    {
        if (isAdmin) return senha ?? string.Empty;
        return new string('*', Math.Min(3, senha?.Length ?? 3));
    }
}