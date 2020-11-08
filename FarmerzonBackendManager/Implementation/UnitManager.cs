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
    public class UnitManager : AbstractManager<DTO.UnitOutput>, IUnitManager
    {
        public UnitManager(IHttpClientFactory clientFactory, ITokenManager tokenManager) : 
            base(clientFactory, tokenManager)
        {
            // nothing to do here
        }
        
        public async Task<IList<DTO.UnitOutput>> GetEntitiesAsync(long? unitId, string name)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (unitId != null)
            {
                query.Add(nameof(unitId), unitId.Value.ToString());   
            }

            if (!string.IsNullOrEmpty(name))
            {
                query.Add(nameof(name), name);
            }
            
            var httpClient = ClientFactory.CreateClient(FarmerzonArticles);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.Token);
            var builder = new UriBuilder($"{httpClient.BaseAddress}unit")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await httpClient.GetAsync(builder.ToString());
            
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            var units = JsonConvert.DeserializeObject<DTO.SuccessResponse<IList<DTO.UnitOutput>>>(httpResponseContent);
            return units.Content;
        }

        public async Task<IDictionary<long, DTO.UnitOutput>> GetUnitsByArticleIdAsync(IEnumerable<long> articleIds)
        {
            return await GetEntitiesByReferenceIdAsDictAsync(articleIds, nameof(articleIds), FarmerzonArticles,
                "unit/get-by-article-id"); 
        }
    }
}