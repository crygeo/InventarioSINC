using Cliente.src.Model;
using Cliente.src.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Cliente.src.ServicesHub
{
    public class UsuarioHubService : HubServiceBase<Usuario>
    {
        public override ObservableCollection<Usuario> Collection { get; }

        public UsuarioHubService(ObservableCollection<Usuario> collection) : base()
        {
            Collection = collection;
        }

    }
}
