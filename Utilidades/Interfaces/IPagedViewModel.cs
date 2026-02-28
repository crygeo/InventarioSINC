using System.Threading.Tasks;

namespace Utilidades.Interfaces;

public interface IPagedViewModel
{
    int PageIndex { get; }
    int PageSize { get; }
    int TotalItems { get; }

    Task LoadPageAsync(int pageIndex);
    Task NextPageAsync();
    Task PrevPageAsync();
}
