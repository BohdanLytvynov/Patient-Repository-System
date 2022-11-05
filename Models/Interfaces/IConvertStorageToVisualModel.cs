using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    public interface IConvertStorageToVisualModel<TStorageModel, TVisualModel>
    {
        TVisualModel StorageToVisualModel();

        TStorageModel VisualToStorageModel();
    }
}
