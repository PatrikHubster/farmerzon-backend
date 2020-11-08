using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using FarmerzonBackendManager.Interface;
using Newtonsoft.Json;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public class StateManager : AbstractManager<DTO.StateOutput>, IStateManager
    {
        public StateManager(IHttpClientFactory clientFactory, ITokenManager tokenManager) : 
            base(clientFactory, tokenManager)
        {
            // nothing to do here
        }

        public async Task<IList<DTO.StateOutput>> GetEntitiesAsync(long? stateId, string name)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (stateId != null)
            {
                query.Add(nameof(stateId), stateId.Value.ToString());
            }

            if (!string.IsNullOrEmpty(name))
            {
                query.Add(nameof(name), name);
            }

            var httpClient = ClientFactory.CreateClient(FarmerzonAddress);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.Token);
            var builder = new UriBuilder($"{httpClient.BaseAddress}state")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await httpClient.GetAsync(builder.ToString());

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            var states = JsonConvert.DeserializeObject<DTO.SuccessResponse<IList<DTO.StateOutput>>>(httpResponseContent);
            return states.Content;
        }

        public async Task<IDictionary<long, DTO.StateOutput>> GetStatesByAddressIdAsync(IEnumerable<long> addressIds)
        {
            return await GetEntitiesByReferenceIdAsDictAsync(addressIds, nameof(addressIds), FarmerzonAddress,
                "state/get-by-address-id");
        }
    }
}