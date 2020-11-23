using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr.Client;
using FarmerzonBackendManager.Interface;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public class CityManager : AbstractManager, ICityManager
    {
        private const string AddressServiceName = "farmerzon-address";
        private const string CityResource = "city";
        private const string CityByAddressEndpoint = "get-by-address-id";

        public CityManager(ITokenManager tokenManager, DaprClient daprClient) : base(tokenManager, daprClient)
        {
            // nothing to do here
        }

        public async Task<IList<DTO.CityOutput>> GetEntitiesAsync(long? cityId, string zipCode, string name)
        {
            IDictionary<string, string> queryParameters = new Dictionary<string, string>();
            if (cityId != null)
            {
                queryParameters.Add(nameof(cityId), cityId.Value.ToString());
            }

            if (!string.IsNullOrEmpty(zipCode))
            {
                queryParameters.Add(nameof(zipCode), zipCode);
            }

            if (!string.IsNullOrEmpty(name))
            {
                queryParameters.Add(nameof(name), name);
            }

            var result =
                await GetEntitiesAsync<DTO.SuccessResponse<IList<DTO.CityOutput>>>(queryParameters, AddressServiceName, 
                    CityResource);
            return result?.Content;
        }

        public async Task<IDictionary<long, DTO.CityOutput>> GetCitiesByAddressIdAsync(IEnumerable<long> addressIds)
        {
            return await GetEntitiesByReferenceIdAsDictionaryAsync<long, DTO.CityOutput>(addressIds,
                nameof(addressIds), AddressServiceName, $"{CityResource}/{CityByAddressEndpoint}");
        }
    }
}