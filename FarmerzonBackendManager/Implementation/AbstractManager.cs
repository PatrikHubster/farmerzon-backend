using System;
using System.Collections.Generic;
using System.Linq;
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
    public abstract class AbstractManager<T>
    {
        protected string FarmerzonAddress { get; private set; }
        protected string FarmerzonArticles { get; private set; }
        protected IHttpClientFactory ClientFactory { get; set; }
        protected ITokenManager TokenManager { get; set; }

        public AbstractManager(IHttpClientFactory clientFactory, ITokenManager tokenManager)
        {
            ClientFactory = clientFactory;
            TokenManager = tokenManager;
            FarmerzonAddress = "FarmerzonAddress";
            FarmerzonArticles = "FarmerzonArticles";
        }

        private async Task<string> GetEntitiesByReferenceIdsAsStringAsync<D>(IEnumerable<D> referenceIds, 
            string referenceName, string serviceName, string serviceEndpoint)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (referenceIds != null)
            {
                foreach (var referenceId in referenceIds)
                {
                    query.Add(referenceName, referenceId.ToString());
                }
            }

            var httpClient = ClientFactory.CreateClient(serviceName);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.Token);
            var builder = new UriBuilder($"{httpClient.BaseAddress}{serviceEndpoint}")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await httpClient.GetAsync(builder.ToString());
            
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return await httpResponse.Content.ReadAsStringAsync();
        }
        
        protected async Task<ILookup<D, T>> GetEntitiesByReferenceIdAsLookupAsync<D>(IEnumerable<D> referenceIds, 
            string referenceName, string serviceName, string serviceEndpoint) where D : IConvertible
        {
            var httpResponseContent =
                await GetEntitiesByReferenceIdsAsStringAsync(referenceIds, referenceName, serviceName, serviceEndpoint);
            var entities = JsonConvert.DeserializeObject<DTO.DictionaryResponse<IList<T>>>(httpResponseContent);
            return entities.Content
                .SelectMany(x => x.Value, Tuple.Create)
                .ToLookup(y => (D) Convert.ChangeType(y.Item1.Key, typeof(D)), y => y.Item2);
        }
        
        protected async Task<IDictionary<D, T>> GetEntitiesByReferenceIdAsDictAsync<D>(IEnumerable<D> referenceIds,
            string referenceName, string serviceName, string serviceEndpoint) where D : IConvertible
        {
            var httpResponseContent =
                await GetEntitiesByReferenceIdsAsStringAsync(referenceIds, referenceName, serviceName, serviceEndpoint);
            var entities = JsonConvert.DeserializeObject<DTO.DictionaryResponse<T>>(httpResponseContent);
            return entities.Content.ToDictionary(key => (D) Convert.ChangeType(key.Key, typeof(D)),
                value => value.Value);
        }
    }
}