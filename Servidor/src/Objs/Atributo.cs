using Shared.Interfaces.ModelsBase;

namespace Servidor.src.Objs
{
    public class Atributo : IAtributo
    {
        public string Name { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
