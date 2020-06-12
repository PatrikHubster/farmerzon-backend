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
    public class PersonManager : AbstractManager, IPersonManager
    {
        public PersonManager(IHttpClientFactory clientFactory, ITokenManager tokenManager) : 
            base(clientFactory, tokenManager)
        {
            // nothing to do here
        }

        public async Task<IList<Person>> GetEntitiesAsync(long? personId, string userName, string normalizedUserName)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (personId != null)
            {
                query.Add(nameof(personId), personId.Value.ToString());
            }

            if (!string.IsNullOrEmpty(normalizedUserName))
            {
                query.Add("userName", userName);
            }

            if (!string.IsNullOrEmpty(normalizedUserName))
            {
                query.Add("normalizedUserName", normalizedUserName);
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
            var people = JsonConvert.DeserializeObject<ListResponse<Person>>(httpResponseContent);
            return people.Content;
        }

        public async Task<IDictionary<long, Person>> GetPeopleByArticleIdAsync(IEnumerable<long> articleIds)
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
            var builder = new UriBuilder($"{httpClient.BaseAddress}person/get-by-article-id")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await httpClient.GetAsync(builder.ToString());
            
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            var people = JsonConvert.DeserializeObject<DictionaryResponse<Person>>(httpResponseContent);
            return people.Content.ToDictionary(key => long.Parse(key.Key), value => value.Value);
        }
    }
}