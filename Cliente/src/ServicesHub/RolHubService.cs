using Cliente.src.Model;
using Cliente.src.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Cliente.src.ServicesHub
{
    public class RolHubService : HubServiceBase<Rol>
    {
        public override ObservableCollection<Rol> Collection { get; } = new();

    }
}
