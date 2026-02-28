using System.Threading.Tasks;

namespace Utilidades.Interfaces;

public interface ICrudViewModel
{
    Task CreateAsync();
    Task UpdateAsync();
    Task DeleteAsync();
}
