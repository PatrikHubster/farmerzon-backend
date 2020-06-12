using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using FarmerzonArticlesDataTransferModel;
using FarmerzonBackendDataTransferModel;
using FarmerzonBackendManager.Interface;
using Newtonsoft.Json;

namespace FarmerzonBackendManager.Implementation
{
    public class UnitManager : AbstractManager, IUnitManager
    {
        public UnitManager(IHttpClientFactory clientFactory, ITokenManager tokenManager) : 
            base(clientFactory, tokenManager)
        {
            // nothing to do here
        }
        
        public async Task<IList<Unit>> GetEntitiesAsync(long? unitId, string name)
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
            var units = JsonConvert.DeserializeObject<ListResponse<Unit>>(httpResponseContent);
            return units.Content;
        }

        public async Task<IDictionary<long, Unit>> GetUnitsByArticleIdAsync(IEnumerable<long> articleIds)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (articleIds != null)
            {
                foreach (var articleId in articleIds)
                {
                    query.Add(nameof(articleIds), articleId.ToString());
                }
            }

            var httpClient = ClientFactory.CreateClient(FarmerzonArticles);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.Token);
            var builder = new UriBuilder($"{httpClient.BaseAddress}unit/get-by-article-id")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await httpClient.GetAsync(builder.ToString());
            
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            var units = JsonConvert.DeserializeObject<DictionaryResponse<Unit>>(httpResponseContent);
            return units.Content.ToDictionary(key => long.Parse(key.Key), value => value.Value);
        }
    }
}