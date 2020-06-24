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
    public class PersonManager : AbstractManager<DTO.Person>, IPersonManager
    {
        public PersonManager(IHttpClientFactory clientFactory, ITokenManager tokenManager) : 
            base(clientFactory, tokenManager)
        {
            // nothing to do here
        }

        public async Task<IList<DTO.Person>> GetEntitiesAsync(long? personId, string userName, string normalizedUserName)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (personId != null)
            {
                query.Add(nameof(personId), personId.Value.ToString());
            }

            if (!string.IsNullOrEmpty(normalizedUserName))
            {
                query.Add(nameof(userName), userName);
            }

            if (!string.IsNullOrEmpty(normalizedUserName))
            {
                query.Add(nameof(normalizedUserName), normalizedUserName);
            }

            var httpClient = ClientFactory.CreateClient(FarmerzonArticles);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.Token);
            var builder = new UriBuilder($"{httpClient.BaseAddress}person")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await httpClient.GetAsync(builder.ToString());

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            var people = JsonConvert.DeserializeObject<DTO.SuccessResponse<IList<DTO.Person>>>(httpResponseContent);
            return people.Content;
        }

        public async Task<IDictionary<long, DTO.Person>> GetPeopleByArticleIdAsync(IEnumerable<long> articleIds)
        {
            return await GetEntitiesByReferenceIdAsDictAsync(articleIds, nameof(articleIds), FarmerzonArticles,
                "person/get-by-article-id");
        }
    }
}