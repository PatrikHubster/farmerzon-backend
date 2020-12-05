using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using FarmerzonBackendManager.Interface;
using Microsoft.Extensions.Logging;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public class StateManager : AbstractManager, IStateManager
    {
        private const string AddressServiceName = "farmerzon-address";
        private const string StateResource = "state";
        private const string StateByAddressEndpoint = "get-by-address-id";

        public StateManager(ITokenManager tokenManager, DaprClient daprClient, 
            ILogger<StateManager> logger) : base(tokenManager, daprClient, logger)
        {
            // nothing to do here
        }

        public async Task<IList<DTO.StateOutput>> GetEntitiesAsync(long? stateId, string name)
        {
            IDictionary<string, string> queryParameters = new Dictionary<string, string>();
            if (stateId != null)
            {
                queryParameters.Add(nameof(stateId), stateId.Value.ToString());
            }

            if (!string.IsNullOrEmpty(name))
            {
                queryParameters.Add(nameof(name), name);
            }

            var result = await InvokeMethodAsync<DTO.SuccessResponse<IList<DTO.StateOutput>>>(AddressServiceName,
                StateResource, HTTPVerb.Get, queryParameters: queryParameters);
            return result?.Content;
        }

        public async Task<IDictionary<long, DTO.StateOutput>> GetStatesByAddressIdAsync(IEnumerable<long> addressIds)
        {
            return await GetEntitiesByReferenceIdAsDictionaryAsync<long, DTO.StateOutput>(addressIds,
                AddressServiceName, $"{StateResource}/{StateByAddressEndpoint}");
        }
    }
}