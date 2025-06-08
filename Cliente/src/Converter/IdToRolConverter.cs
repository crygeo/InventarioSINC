using Cliente.Obj.Model;
using Cliente.Services;
using Cliente.src.Services.Model;
using Utilidades.Converters.Generic;

namespace Cliente.Converter;

public class IdToRolConverter : IdToClassifiedConvert<Rol>
{
    public IdToRolConverter() : base(id => ServiceFactory.GetService<Rol>().GetById(id)) { }

}