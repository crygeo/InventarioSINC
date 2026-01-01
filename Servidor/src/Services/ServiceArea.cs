using System.Threading.Tasks;
using Servidor.HubsService;
using Servidor.Model;
using Servidor.Repositorios;

namespace Servidor.Services;

public class ServiceArea : ServiceListProperty<Area>
{
    public RepositorioArea RepositorioArea => (RepositorioArea)Repository;
    public HubServiceArea HubServiceArea => (HubServiceArea)HubService;
    
}