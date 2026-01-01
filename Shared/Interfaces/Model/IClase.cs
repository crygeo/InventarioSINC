namespace Shared.Interfaces.ModelsBase;

[Obsolete("Esta clase está deshabilitada. No debe usarse.", true)]
public interface IClase : IIdentifiable, IDeleteable
{
    string Name { get; set; }
    string Description { get; set; }
}