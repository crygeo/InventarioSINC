using Cliente.src.Model;
using Cliente.src.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Utilidades.Converters.Generic;

namespace Cliente.src.Converter
{
    public class IdToRolConverter : IdToClassifiedConvert<Rol>
    {
        public IdToRolConverter() : base(id => RolService.Instance.ObtenerPorId(id)) { }

    }

}
