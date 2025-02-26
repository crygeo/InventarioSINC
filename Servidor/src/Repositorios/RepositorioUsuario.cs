using MongoDB.Driver;
using Servidor.src.Objs;

namespace Servidor.src.Repositorios
{
    public class RepositorioUsuario : RepositorioBase<Usuario>
    {

        public override string NameCollection { get; set; } = "Usuarios";

        public RepositorioUsuario() : base()
        {
        }
        /// <summary>
        /// Obtiene el objeto Usuario por el nombre del usuario
        /// </summary>
        /// <param name="user">Nombre de usuario</param>
        /// <returns>Usuario a buscar.</returns>
        public async Task<Usuario> GetByUser(string user)
        {
            return await Collection.Find(u => u.User == user).FirstOrDefaultAsync();
        }
    }
}
