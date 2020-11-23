using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr.Client;
using FarmerzonBackendManager.Interface;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public class CountryManager : AbstractManager, ICountryManager
    {
        private const string AddressServiceName = "farmerzon-address";
        private const string CountryResource = "country";
        private const string CountryByAddressEndpoint = "get-by-address-id";
        
        public CountryManager(ITokenManager tokenManager, DaprClient daprClient) : base(tokenManager, daprClient)
        {
            // nothing to do here
        }

        public async Task<IList<DTO.CountryOutput>> GetEntitiesAsync(long? countryId, string name, string code)
        {
            IDictionary<string, string> queryParameters = new Dictionary<string, string>();
            if (countryId != null)
            {
                queryParameters.Add(nameof(countryId), countryId.Value.ToString());
            }

            if (!string.IsNullOrEmpty(name))
            {
                queryParameters.Add(nameof(name), name);
            }

            if (!string.IsNullOrEmpty(name))
            {
                queryParameters.Add(nameof(code), code);
            }

            var result =
                await GetEntitiesAsync<DTO.SuccessResponse<IList<DTO.CountryOutput>>>(queryParameters,
                    AddressServiceName, CountryResource);
            return result?.Content;
        }

        public async Task<IDictionary<long, DTO.CountryOutput>> GetCountriesByAddressIdAsync(
            IEnumerable<long> addressIds)
        {
            return await GetEntitiesByReferenceIdAsDictionaryAsync<long, DTO.CountryOutput>(addressIds,
                nameof(addressIds), AddressServiceName, $"{CountryResource}/{CountryByAddressEndpoint}");
        }
    }
}