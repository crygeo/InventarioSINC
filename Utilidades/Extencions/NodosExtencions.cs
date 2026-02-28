using System.Collections.Generic;
using System.Linq;
using Utilidades.Factory;

namespace Utilidades.Extencions;

public static class NodosExtencions
{
    public static List<Nodos> ObtenerSeleccionados(this IEnumerable<Nodos> nodos)
    {
        var resultado = new List<Nodos>();

        foreach (var nodo in nodos)
            if (nodo.IsChecked == true || nodo.Hijos.Any(h => h.IsChecked == true || h.Hijos.Any()))
            {
                var nodoClonado = new Nodos
                {
                    Nombre = nodo.Nombre,
                    IsChecked = nodo.IsChecked,
                    Hijos = nodo.Hijos.ObtenerSeleccionados()
                };

                foreach (var hijo in nodoClonado.Hijos)
                    hijo.Padre = nodoClonado;

                resultado.Add(nodoClonado);
            }

        return resultado;
    }

    public static List<Nodos> ConstruirArbol(this IEnumerable<string> permisos, char separator = '.')
    {
        var raiz = new List<Nodos>();

        foreach (var permiso in permisos)
        {
            var partes = permiso.Split(separator);
            AgregarNodo(raiz, partes, 0);
        }

        return raiz;
    }

    public static List<string> DescostruirArbol(this IEnumerable<Nodos> nodos, string rutaPadre = "")
    {
        var resultado = new List<string>();

        foreach (var nodo in nodos)
        {
            var rutaActual = string.IsNullOrEmpty(rutaPadre)
                ? nodo.Nombre
                : $"{rutaPadre}.{nodo.Nombre}";

            if (nodo.Hijos?.Any() == true)
                resultado.AddRange(nodo.Hijos.DescostruirArbol(rutaActual));
            else
                resultado.Add(rutaActual);
        }

        return resultado;
    }

    public static List<Nodos> SeleccionarNodos(this IEnumerable<Nodos> nodos, IEnumerable<string> permisos,
        string rutaPadre = "")
    {
        foreach (var nodo in nodos)
        {
            var rutaActual = string.IsNullOrEmpty(rutaPadre)
                ? nodo.Nombre
                : $"{rutaPadre}.{nodo.Nombre}";

            if (permisos.Contains(rutaActual)) nodo.IsChecked = true;

            // Recorre los hijos recursivamente
            nodo.Hijos.SeleccionarNodos(permisos, rutaActual);
        }

        return nodos.ToList();
    }

    private static void AgregarNodo(List<Nodos> nodos, string[] partes, int nivel, Nodos? padre = null)
    {
        if (nivel >= partes.Length)
            return;

        var nombreActual = partes[nivel];
        var nodo = nodos.FirstOrDefault(n => n.Nombre == nombreActual);
        if (nodo == null)
        {
            nodo = new Nodos { Nombre = nombreActual, Padre = padre };
            nodos.Add(nodo);
        }

        AgregarNodo(nodo.Hijos, partes, nivel + 1, nodo);
    }
}