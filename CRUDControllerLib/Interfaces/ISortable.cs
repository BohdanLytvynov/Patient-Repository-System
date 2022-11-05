using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDControllerLib.Interfaces
{
    public interface ISortable<TItem>
        
    {
        Task SortAsync(List<TItem> col, IComparer<TItem> comparer);
    }
}
