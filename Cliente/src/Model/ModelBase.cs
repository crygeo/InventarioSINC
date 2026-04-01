using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cliente.Attributes;
using Cliente.Helpers;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Utilidades.Attributes;
using Utilidades.Interfaces;

namespace Cliente.Obj.Model;

public abstract class ModelBase<T> : ModelBase, ISelectable, IModelObj where T : IModelObj
{
    private bool _deleteable = true;

    private string _id = "";
    private bool _isSelect;
    private bool _isSelectable = true;
    private bool _updatable;
    private bool _verView;

    public bool VerView
    {
        get => _verView;
        set => SetProperty(ref _verView, value);
    }

    [NoActualizar]
    public string Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    public bool Deleteable
    {
        get => _deleteable;
        set => SetProperty(ref _deleteable, value);
    }

    public bool Updatable
    {
        get => _updatable;
        set => SetProperty(ref _updatable, value);
    }

    public void Update(IModelObj entity)
    {
        if (entity is T source)
            if (this is T target)
                target.UpdateFrom(source);
    }

    public bool IsSelectable
    {
        get => _isSelectable;
        set => SetProperty(ref _isSelectable, value);
    }

    public bool IsSelect
    {
        get => _isSelect;
        set => SetProperty(ref _isSelect, value);
    }

}

public abstract class ModelBase : NotifyProperty, INotifyDataErrorInfo, IRemoteUpdatable
{
    public event Action? OnRemoteUpdated;

    private readonly Dictionary<string, List<string>> _errores = new();

    public bool HasErrors => _errores.Any();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName)) return _errores.SelectMany(e => e.Value);
        return _errores.TryGetValue(propertyName, out var errores) ? errores : Enumerable.Empty<string>();
    }

    protected override bool SetProperty<T1>(ref T1 field, T1 value, [CallerMemberName] string? propertyName = null)
    {
        
        var cambiado = base.SetProperty(ref field, value, propertyName);
        if (cambiado)
            Validar(propertyName!);
        return cambiado;
    }

    private void Validar(string propertyName)
    {
        _errores.Remove(propertyName);

        var nuevosErrores = this.ValidarPropiedadAtributo(propertyName);

        if (nuevosErrores.Any())
            _errores[propertyName] = nuevosErrores;

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    protected override void UpdateChanged()
    {
        OnRemoteUpdated?.Invoke();
    }
}