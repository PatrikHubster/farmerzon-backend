using System.Collections.Generic;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface ICountryManager
    {
        public Task<IList<DTO.Country>> GetEntitiesAsync(long? countryId, string name, string code);
        public Task<IDictionary<long, DTO.Country>> GetCountriesByAddressIdAsync(IEnumerable<long> addressIds);
    }
}