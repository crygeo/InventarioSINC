using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilidades.Factory;

public class Nodos : NotifyProperty
{
    private bool? _isChecked = false;
    public string Nombre { get; set; } = string.Empty;
    public List<Nodos> Hijos { get; set; } = new();

    public bool? IsChecked
    {
        get => _isChecked;
        set
        {
            if (_isChecked != value)
            {
                _isChecked = value;
                OnPropertyChanged();

                // Propagar a los hijos si el valor no es nulo
                if (_isChecked.HasValue)
                {
                    foreach (var hijo in Hijos) hijo.IsChecked = value;

                    Padre?.ActualizarEstadoDesdeHijos();
                }
            }
        }
    }

    public Nodos? Padre { get; set; }

    public void ActualizarEstadoDesdeHijos()
    {
        if (Hijos.All(h => h.IsChecked == true))
            _isChecked = true;
        else if (Hijos.All(h => h.IsChecked == false))
            _isChecked = false;
        else
            _isChecked = null;

        OnPropertyChanged(nameof(IsChecked));

        // Aquí está el cambio importante:
        Padre?.ActualizarEstadoDesdeHijos();
    }

    protected override void UpdateChanged()
    {
        throw new NotImplementedException();
    }
}