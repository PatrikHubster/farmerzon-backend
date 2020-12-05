using System.Collections.Generic;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface ICityManager
    {
        public Task<IList<DTO.CityOutput>> GetEntitiesAsync(long? cityId, string zipCode, string name);
        public Task<IDictionary<long, DTO.CityOutput>> GetCitiesByAddressIdAsync(IEnumerable<long> addressIds);
    }
}