using Servidor.src.Objs;

namespace Servidor.src.Repositorios
{
    public class RepositorioRol : RepositorioBase<Rol>
    {

        public override string NameCollection { get; set; } = "Roles";

        public RepositorioRol() : base()
        {
        }
    }
}
