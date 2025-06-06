using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Servidor.src.Model
{
    public class ElementoJerarquico : IElementoJerarquico
    {
        public string Id { get; set; }
        public string IdPerteneciente { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public bool Updatable { get; set; }
        public bool Deleteable { get; set; }
        public void Update(IModelObj entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
