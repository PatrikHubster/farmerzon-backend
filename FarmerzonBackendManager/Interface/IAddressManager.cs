using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface IAddressManager
    {
        public Task<IList<DTO.AddressOutput>> GetEntitiesAsync(long? addressId, string doorNumber, string street);
        public Task<ILookup<long, DTO.AddressOutput>> GetAddressesByCityIdAsync(IEnumerable<long> cityIds);
        public Task<ILookup<long, DTO.AddressOutput>> GetAddressesByCountryIdAsync(IEnumerable<long> countryIds);
        public Task<ILookup<long, DTO.AddressOutput>> GetAddressesByStateIdAsync(IEnumerable<long> stateIds);
        public Task<IDictionary<string, DTO.AddressOutput>> GetAddressesByNormalizedUserName(
            IEnumerable<string> normalizedUserNames);
    }
}