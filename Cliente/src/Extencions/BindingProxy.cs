using System.Windows;
using Cliente.ViewModel.Model;

namespace Cliente.Extencions;

public class BindingProxy : Freezable
{
    public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy), new PropertyMetadata(null));

    public object Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    protected override Freezable CreateInstanceCore()
    {
        return new BindingProxy();
    }
}

public class BindingProxy<T> : Freezable
{
    protected override Freezable CreateInstanceCore()
        => new BindingProxy<T>();

    public T Data
    {
        get => (T)GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(T), typeof(BindingProxy<T>));
}

public class BindingProxyWrapper : BindingProxy<PageUsuarioVM>
{
}