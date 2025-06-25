﻿using Cliente.Obj.Model;
using Cliente.View.Items;
using System.Windows.Controls.Primitives;

namespace Cliente.Services.Model;

public class ServiceElementoJerarquico : ServiceBase<ElementoJerarquico>
{
    public Dictionary<Identificador, List<ElementoJerarquico>> GetAllIdentificadoresList()
    {
        var identificadores = ServiceFactory.GetService<Identificador>().Collection.ToList().OrderBy(g => g.FechaCreacion);
        var elementos = Collection;

        // Creamos un diccionario auxiliar para agrupar rápido por ID
        var agrupados = elementos
            .GroupBy(e => e.IdPerteneciente)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(e => e.FechaCreacion).ToList());

        // Convertimos los ID a objetos Identificador reales
        var resultado = new Dictionary<Identificador, List<ElementoJerarquico>>();

        foreach (var identificador in identificadores)
        {
            if (agrupados.TryGetValue(identificador.Id, out var valores))
            {
                resultado[identificador] = valores;
            }
            else
            {
                resultado[identificador] = new List<ElementoJerarquico>();
            }
        }

        return resultado;
    }

}