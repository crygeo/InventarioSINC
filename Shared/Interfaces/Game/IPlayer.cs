using Shared.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.Game
{
    public interface IPlayer : IModelObj
    {
        public long? PlayerId { get; set; }
        public Guid ObjectId { get; set; }
        public string UserName { get; set; }
        public string GuildName { get; set; }
        public string AlianceName { get; set; }
    }
}
