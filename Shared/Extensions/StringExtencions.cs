using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{
    public static class StringExtencions
    {
        public static void AddOrRemove(this List<string> str, string value)
        {
            if (str.Contains(value))
                str.Remove(value);
            else
                str.Add(value);
        }
    }
}
