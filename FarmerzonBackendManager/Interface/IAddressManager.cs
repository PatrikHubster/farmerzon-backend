using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface IAddressManager
    {
        public Task<IList<DTO.Address>> GetEntitiesAsync(long? addressId, string doorNumber, string street);
        public Task<ILookup<long, DTO.Address>> GetAddressesByCityIdAsync(IEnumerable<long> cityIds);
        public Task<ILookup<long, DTO.Address>> GetAddressesByCountryIdAsync(IEnumerable<long> countryIds);
        public Task<ILookup<long, DTO.Address>> GetAddressesByStateIdAsync(IEnumerable<long> stateIds);
    }
}