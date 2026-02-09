namespace MinimalApi.Dominio.ModelViews;

public struct Home
{
    public string? Message { get => "Bem-vindo à API de Veículos - minimalAPI"; }
    public string? Doc { get => "/swagger";}
}