using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Utilidades.Attributes;

public abstract class NotifyProperty : INotifyPropertyChanged, INotifyPropertyChanging
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event PropertyChangingEventHandler? PropertyChanging;

    protected void OnPropertyChanging([CallerMemberName] string propertyName = "")
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        OnPropertyChanging(propertyName);
        field = value;
        OnPropertyChanged(propertyName);
        UpdateChanged();
        NotifyDependentProperties(propertyName);
        return true;
    }

    protected abstract void UpdateChanged();
    private void NotifyDependentProperties(string? propertyName)
    {
        if (propertyName == null)
            return;

        var deps = GetType()
            .GetProperties()
            .Where(p =>
                p.GetCustomAttributes<DependsOnAttribute>(true)
                    .Any(a => a.PropertyName == propertyName));

        foreach (var prop in deps)
            OnPropertyChanged(prop.Name);
    }

}