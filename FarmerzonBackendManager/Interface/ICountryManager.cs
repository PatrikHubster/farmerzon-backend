using System.Collections.Generic;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface ICountryManager
    {
        public Task<IList<DTO.CountryOutput>> GetEntitiesAsync(long? countryId, string name, string code);
        public Task<IDictionary<long, DTO.CountryOutput>> GetCountriesByAddressIdAsync(IEnumerable<long> addressIds);
    }
}