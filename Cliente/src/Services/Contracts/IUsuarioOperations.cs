
using Utilidades.Interfaces;

namespace Cliente.Services.Contracts;

public interface IUsuarioOperations
{
    Task<IResultResponse<bool>> ChangePasswordAsync(
        string userId, string oldPassword, string newPassword);

    Task<IResultResponse<bool>> AsignarRolAsync(
        string userId, string rolId);
}