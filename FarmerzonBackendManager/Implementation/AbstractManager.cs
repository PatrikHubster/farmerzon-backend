using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using FarmerzonBackendManager.Interface;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public abstract class AbstractManager
    {
        protected ITokenManager TokenManager { get; set; }
        protected DaprClient DaprClient { get; set; }

        public AbstractManager(ITokenManager tokenManager, DaprClient daprClient)
        {
            TokenManager = tokenManager;
            DaprClient = daprClient;
        }

        protected async Task<T> GetEntitiesAsync<T>(IDictionary<string, string> queryParameters, 
            string serviceName, string serviceEndpoint)
        {
            var httpExtension = new HTTPExtension
            {
                ContentType = "application/json", 
                Verb = HTTPVerb.Get
            };
            httpExtension.Headers.Add("Authorization", $"Bearer {TokenManager.Token}");
            httpExtension.QueryString = queryParameters;
            
            return await DaprClient.InvokeMethodAsync<T>(serviceName, serviceEndpoint, httpExtension);
        }
        
        protected async Task<ILookup<T, D>> GetEntitiesByReferenceIdAsLookupsAsync<T, D>(IEnumerable<T> referenceIds, 
            string referenceName, string serviceName, string serviceEndpoint) where T : IConvertible
        {
            IDictionary<string, string> queryParameters = new Dictionary<string, string>();
            foreach (var referenceId in referenceIds)
            {
                queryParameters.Add(referenceName, referenceId.ToString());
            }

            var result =
                await GetEntitiesAsync<DTO.SuccessResponse<Dictionary<string, IList<D>>>>(queryParameters, serviceName,
                    serviceEndpoint);
            return result?.Content
                .SelectMany(x => x.Value, Tuple.Create)
                .ToLookup(y => (T) Convert.ChangeType(y.Item1.Key, typeof(T)), y => y.Item2);
        }
        
        protected async Task<IDictionary<T, D>> GetEntitiesByReferenceIdAsDictionaryAsync<T, D>(IEnumerable<T> referenceIds,
            string referenceName, string serviceName, string serviceEndpoint) where T : IConvertible
        {
            IDictionary<string, string> queryParameters = new Dictionary<string, string>();
            foreach (var referenceId in referenceIds)
            {
                queryParameters.Add(referenceName, referenceId.ToString());
            }

            var result =
                await GetEntitiesAsync<DTO.SuccessResponse<Dictionary<string, D>>>(queryParameters, serviceName,
                    serviceEndpoint);
            return result?.Content.ToDictionary(key => (T) Convert.ChangeType(key.Key, typeof(T)),
                value => value.Value);
        }
    }
}