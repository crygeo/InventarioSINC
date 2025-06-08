using System.Collections.ObjectModel;
using Cliente.Obj.Model;

namespace Cliente.ServicesHub;

public class UsuarioHubService : HubServiceBase<Usuario>
{
    public override ObservableCollection<Usuario> Collection { get; } = new();
}