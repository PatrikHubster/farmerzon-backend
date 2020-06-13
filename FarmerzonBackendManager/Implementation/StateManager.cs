using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using FarmerzonBackendDataTransferModel;
using FarmerzonBackendManager.Interface;
using Newtonsoft.Json;

namespace FarmerzonBackendManager.Implementation
{
    public class StateManager : AbstractManager, IStateManager
    {
        public StateManager(IHttpClientFactory clientFactory, ITokenManager tokenManager) : 
            base(clientFactory, tokenManager)
        {
            // nothing to do here
        }

        public async Task<IList<State>> GetEntitiesAsync(long? stateId, string name)
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
            var builder = new UriBuilder($"{httpClient.BaseAddress}address")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await httpClient.GetAsync(builder.ToString());

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            var states = JsonConvert.DeserializeObject<ListResponse<State>>(httpResponseContent);
            return states.Content;
        }
    }
}