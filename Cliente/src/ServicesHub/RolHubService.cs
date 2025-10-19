using System.Collections.ObjectModel;
using Cliente.Obj.Model;

namespace Cliente.ServicesHub;

public class RolHubService : HubServiceBase<Rol>
{
    public override ObservableCollection<Rol> Collection { get; } = new();

}