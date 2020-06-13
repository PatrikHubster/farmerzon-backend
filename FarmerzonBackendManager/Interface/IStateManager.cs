using System.Collections.Generic;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface IStateManager
    {
        public Task<IList<DTO.State>> GetEntitiesAsync(long? stateId, string name);
    }
}