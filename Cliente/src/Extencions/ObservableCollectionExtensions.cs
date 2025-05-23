using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.src.Extencions
{
    public static class ObservableCollectionExtensions
    {
        public static void ReplaceWith<T>(this ObservableCollection<T> collection, IEnumerable<T> newItems)
        {
            collection.Clear();
            foreach (var item in newItems)
                collection.Add(item);
        }
    }
}
