using Cliente.src.Model;
using Cliente.src.Services.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Cliente.src.Services;
using Utilidades.Converters.Generic;

namespace Cliente.src.Converter
{
    public class IdToRolConverter : IdToClassifiedConvert<Rol>
    {
        public IdToRolConverter() : base(id => ServiceFactory.GetService<Rol>().GetById(id)) { }

    }

}
