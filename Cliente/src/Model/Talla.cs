using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Interfaces;

namespace Cliente.src.Model
{
    public class Talla : ModelBase<ITalla>, ITalla
    {

        private string _id = string.Empty;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private List<IClase> _clasesPermitidas = new List<IClase>();

        public override string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string Name
        {
            get => _name; 
            set => SetProperty(ref _name, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public List<IClase> ClasesPermitidas
        {
            get => _clasesPermitidas;
            set => SetProperty(ref _clasesPermitidas, value);
        }

        public override void Update(IIdentifiable identity)
        {
            if(identity is not ITalla talla)
                throw new ArgumentException("El objeto no es una talla");

            Id = talla.Id;
            Name = talla.Name;
            Description = talla.Description;
            ClasesPermitidas = talla.ClasesPermitidas;
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }

    
}
