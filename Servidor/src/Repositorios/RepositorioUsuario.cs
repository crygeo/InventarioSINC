using System.Threading.Tasks;
using MongoDB.Driver;
using Servidor.Model;

namespace Servidor.Repositorios;

public class RepositorioUsuario : RepositorioBase<Usuario>
{
    /// <summary>
    ///     Obtiene el objeto Usuario por el nombre del usuario
    /// </summary>
    /// <param name="user">Nombre de usuario</param>
    /// <returns>Usuario a buscar.</returns>
    public async Task<Usuario> GetByUser(string user)
    {
        return await Collection.Find(u => u.User == user).FirstOrDefaultAsync();
    }

    public async Task<bool> ActualizarPassword(string idUser, string newPassword)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(newPassword);

        var filter = Builders<Usuario>.Filter.Eq(u => u.Id, idUser);
        var update = Builders<Usuario>.Update.Set(u => u.Password, hash);
        var result = await Collection.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }
}