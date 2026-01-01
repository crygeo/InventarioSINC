using MaterialDesignThemes.Wpf;

namespace Cliente.Attributes;

public class NavegacionAttribute(string id) : Attribute
{
    public string Indicador { get; set; } = id;
    public required string TituloS { get; set; }
    public string TituloP { get; set; } = string.Empty;
    public required PackIconKind SelectedIcon { get; set; }
    public required PackIconKind UnselectedIcon { get; set; }
    public Type? DialogoPersonalizado { get; set; }

    public int Notification { get; set; } = 0;
}