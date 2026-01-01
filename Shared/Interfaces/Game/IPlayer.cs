using Shared.Interfaces.Model;

namespace Shared.Interfaces.Game;

public interface IPlayer : IModelObj
{
    public long? PlayerId { get; set; }
    public Guid ObjectId { get; set; }
    public string UserName { get; set; }
    public string GuildName { get; set; }
    public string AlianceName { get; set; }
}