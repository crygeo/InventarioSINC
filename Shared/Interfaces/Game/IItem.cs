using Shared.Enums;
using Shared.Interfaces.Model;

namespace Shared.Interfaces.Game;

public interface IItem : IModelObj
{
    public int Index { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IdItemVariable { get; set; }
    public byte[] Imagen { get; set; }
    public TypeItem Type { get; set; }
    public TierType Tier { get; set; }
    public EncanType Encan { get; set; }
    public Categoria[] ShopCategorias { get; set; }
}