using System.Collections.Generic;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface IStateManager
    {
        public Task<IList<DTO.StateOutput>> GetEntitiesAsync(long? stateId, string name);
        public Task<IDictionary<long, DTO.StateOutput>> GetStatesByAddressIdAsync(IEnumerable<long> addressIds);
    }
}