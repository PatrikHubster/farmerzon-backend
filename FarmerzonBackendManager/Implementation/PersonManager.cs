using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public PersonManager(IHttpClientFactory clientFactory) : base(clientFactory)
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

            var client = ClientFactory.CreateClient(FarmerzonArticles);
            var builder = new UriBuilder($"{client.BaseAddress}person")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await client.GetAsync(builder.ToString());

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

            var client = ClientFactory.CreateClient(FarmerzonArticles);
            var builder = new UriBuilder($"{client.BaseAddress}person/get-by-article-id")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await client.GetAsync(builder.ToString());
            
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