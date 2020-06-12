using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class ArticleManager : AbstractManager, IArticleManager
    {
        public ArticleManager(IHttpClientFactory clientFactory, ITokenManager tokenManager) : 
            base(clientFactory, tokenManager)
        {
            // nothing to do here
        }
        
        public async Task<IList<Article>> GetEntitiesAsync(long? articleId, string name, string description, 
            double? price, int? amount, double? size, DateTime? createdAt, DateTime? updatedAt)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (articleId != null)
            {
                query.Add(nameof(articleId), articleId.Value.ToString());
            }

            if (!string.IsNullOrEmpty(name))
            {
                query.Add(nameof(name), name);
            }

            if (!string.IsNullOrEmpty(description))
            {
                query.Add(nameof(description), description);
            }

            if (price != null)
            {
                query.Add(nameof(price), price.Value.ToString("R"));
            }

            if (amount != null)
            {
                query.Add(nameof(amount), amount.Value.ToString());
            }

            if (size != null)
            {
                query.Add(nameof(size), size.Value.ToString("R"));
            }

            if (createdAt != null)
            {
                query.Add(nameof(createdAt), createdAt.Value.ToString(CultureInfo.CurrentCulture));
            }

            if (updatedAt != null)
            {
                query.Add(nameof(updatedAt), updatedAt.Value.ToString(CultureInfo.CurrentCulture));
            }

            var httpClient = ClientFactory.CreateClient(FarmerzonArticles);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.Token);
            var builder = new UriBuilder($"{httpClient.BaseAddress}article")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await httpClient.GetAsync(builder.ToString());

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            var articles = JsonConvert.DeserializeObject<ListResponse<Article>>(httpResponseContent);
            return articles.Content;
        }

        public async Task<ILookup<long, Article>> GetArticlesByPersonIdAsync(IEnumerable<long> personIds)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (personIds != null)
            {
                foreach (var personId in personIds)
                {
                    query.Add(nameof(personIds), personId.ToString());
                }
            }

            var httpClient = ClientFactory.CreateClient(FarmerzonArticles);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.Token);
            var builder = new UriBuilder($"{httpClient.BaseAddress}article/get-by-person-id")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await httpClient.GetAsync(builder.ToString());

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            
            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            var articles = JsonConvert.DeserializeObject<DictionaryResponse<IList<Article>>>(httpResponseContent);
            return articles.Content
                .SelectMany(x => x.Value, Tuple.Create)
                .ToLookup(p => long.Parse(p.Item1.Key), p => p.Item2);
        }

        public async Task<ILookup<long, Article>> GetArticlesByUnitIdAsync(IEnumerable<long> unitIds)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (unitIds != null)
            {
                foreach (var unitId in unitIds)
                {
                    query.Add(nameof(unitIds), unitId.ToString());
                }
            }

            var httpClient = ClientFactory.CreateClient(FarmerzonArticles);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.Token);
            var builder = new UriBuilder($"{httpClient.BaseAddress}article/get-by-unit-id")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await httpClient.GetAsync(builder.ToString());
            
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            var articles = JsonConvert.DeserializeObject<DictionaryResponse<IList<Article>>>(httpResponseContent);
            return articles.Content
                .SelectMany(x => x.Value, Tuple.Create)
                .ToLookup(p => long.Parse(p.Item1.Key), p => p.Item2);
        }
    }
}