using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using FarmerzonBackendManager.Interface;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public class AddressManager : AbstractManager, IAddressManager
    {
        private const string AddressServiceName = "farmerzon-address";
        private const string AddressResource = "address";
        private const string AddressByCityEndpoint = "get-by-city-id";
        private const string AddressByCountryEndpoint = "get-by-country-id";
        private const string AddressByStateEndpoint = "get-by-state-id";
        private const string AddressByNormalizedUserNameEndpoint = "get-by-normalized-user-name";
        
        public AddressManager(ITokenManager tokenManager, DaprClient daprClient) : base(tokenManager, daprClient)
        {
            // nothing to do here
        }

        public async Task<IList<DTO.AddressOutput>> GetEntitiesAsync(long? addressId, string doorNumber, string street)
        {
            IDictionary<string, string> queryParameters = new Dictionary<string, string>();
            if (addressId != null)
            {
                queryParameters.Add(nameof(addressId), addressId.Value.ToString());
            }

            if (!string.IsNullOrEmpty(doorNumber))
            {
                queryParameters.Add(nameof(doorNumber), doorNumber);
            }

            if (!string.IsNullOrEmpty(street))
            {
                queryParameters.Add(nameof(street), street);
            }

            var result =
                await GetEntitiesAsync<DTO.SuccessResponse<IList<DTO.AddressOutput>>>(queryParameters,
                    AddressServiceName, AddressResource);
            return result?.Content;
        }

        public async Task<ILookup<long, DTO.AddressOutput>> GetAddressesByCityIdAsync(IEnumerable<long> cityIds)
        {
            return await GetEntitiesByReferenceIdAsLookupsAsync<long, DTO.AddressOutput>(cityIds, 
                nameof(cityIds), AddressServiceName, $"{AddressResource}/{AddressByCityEndpoint}");
        }

        public async Task<ILookup<long, DTO.AddressOutput>> GetAddressesByCountryIdAsync(IEnumerable<long> countryIds)
        {
            return await GetEntitiesByReferenceIdAsLookupsAsync<long, DTO.AddressOutput>(countryIds,
                nameof(countryIds), AddressServiceName, $"{AddressResource}/{AddressByCountryEndpoint}");
        }

        public async Task<ILookup<long, DTO.AddressOutput>> GetAddressesByStateIdAsync(IEnumerable<long> stateIds)
        {
            return await GetEntitiesByReferenceIdAsLookupsAsync<long, DTO.AddressOutput>(stateIds,
                nameof(stateIds), AddressServiceName, $"{AddressResource}/{AddressByStateEndpoint}");
        }

        public async Task<ILookup<string, DTO.AddressOutput>> GetAddressesByNormalizedUserNameAsync(
            IEnumerable<string> normalizedUserNames)
        {
            return await GetEntitiesByReferenceIdAsLookupsAsync<string, DTO.AddressOutput>(
                normalizedUserNames, nameof(normalizedUserNames), AddressServiceName,
                $"{AddressResource}/{AddressByNormalizedUserNameEndpoint}");
        }
    }
}