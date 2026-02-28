using Utilidades.Interfaces;

namespace Utilidades.Factory;

public static class ClassifiedFactory
{
    public static T CrearClasificado<T>(string mensaje = "Clasificado")
        where T : IClassified, new()
    {
        return new T
        {
            IsClassified = true,
            Message = mensaje
        };
    }

    public static class Mensajes
    {
        public const string Clasificado = "Clasificado";
        public const string Invalido = "Rol inválido";

        public const string SinPermiso = "Sin permisos para ver este elemento";
        // Agrega más si lo deseas
    }
}