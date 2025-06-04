using Shared.Interfaces.ModelsBase;
using System.Collections.Generic;
using System.Linq;

namespace Servidor.src.Objs
{
    public class AtributosEntity : IAtributosEntity
    {
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Descripcion { get; set; }
        public IEnumerable<Atributo> Atributos { get; set; } = new List<Atributo>();

        IEnumerable<IAtributo> IAtributosEntity.Atributos
        {
            get => Atributos;
            set => Atributos = value.Cast<Atributo>();
        }
    }
}
