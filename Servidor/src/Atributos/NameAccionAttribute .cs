namespace Servidor.src.Atributos
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NameAccionAttribute : Attribute
    {
        public string Accion { get; }

        public NameAccionAttribute(string accion)
        {
            Accion = accion;
        }
    }

}
