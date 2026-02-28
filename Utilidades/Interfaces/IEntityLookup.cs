using System;
using System.Collections.Generic;

namespace Utilidades.Interfaces;

public interface IEntityLookup
{
    string GetDisplayName(Type entityType, string id);
    string GetDisplayName(Type entityType, IEnumerable<string> ids);
    string GetDisplayName(Type entityType, object ids);
}