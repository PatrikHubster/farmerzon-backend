using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using FarmerzonBackendManager.Interface;
using Microsoft.Extensions.Logging;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public class CityManager : AbstractManager, ICityManager
    {
        private const string AddressServiceName = "farmerzon-address";
        private const string CityResource = "city";
        private const string CityByAddressEndpoint = "get-by-address-id";

        public CityManager(ITokenManager tokenManager, DaprClient daprClient, 
            ILogger<CityManager> logger) : base(tokenManager, daprClient, logger)
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

            var result = await InvokeMethodAsync<DTO.SuccessResponse<IList<DTO.CityOutput>>>(AddressServiceName,
                CityResource, HTTPVerb.Get, queryParameters: queryParameters);
            return result?.Content;
        }

        public async Task<IDictionary<long, DTO.CityOutput>> GetCitiesByAddressIdAsync(IEnumerable<long> addressIds)
        {
            return await GetEntitiesByReferenceIdAsDictionaryAsync<long, DTO.CityOutput>(addressIds, AddressServiceName,
                $"{CityResource}/{CityByAddressEndpoint}");
        }
    }
}