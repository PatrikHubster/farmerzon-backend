using System.Collections.Generic;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface ICityManager
    {
        public Task<IList<DTO.City>> GetEntitiesAsync(long? cityId, string zipCode, string name);
        public Task<IDictionary<long, DTO.City>> GetCitiesByAddressIdAsync(IEnumerable<long> addressIds);
    }
}