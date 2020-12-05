using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using FarmerzonBackendManager.Interface;
using Microsoft.Extensions.Logging;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public abstract class AbstractManager
    {
        protected ITokenManager TokenManager { get; set; }
        protected DaprClient DaprClient { get; set; }
        protected ILogger Logger { get; set; }

        public AbstractManager(ITokenManager tokenManager, DaprClient daprClient, ILogger logger)
        {
            TokenManager = tokenManager;
            DaprClient = daprClient;
            Logger = logger;
        }

        private HTTPExtension GenerateExtension(HTTPVerb type, IDictionary<string, string> queryParameters = null)
        {
            var httpExtension = new HTTPExtension
            {
                ContentType = "application/json",
                Verb = type
            };
            httpExtension.Headers.Add("Authorization", $"Bearer {TokenManager.Token}");

            if (queryParameters != null)
            {
                httpExtension.QueryString = queryParameters;
            }

            return httpExtension;
        }
        
        protected async Task<TOut> InvokeMethodAsync<TOut>(string serviceName, string serviceEndpoint, HTTPVerb type,
            IDictionary<string, string> queryParameters = null)
        {
            var httpExtension = GenerateExtension(type, queryParameters: queryParameters);
            return await DaprClient.InvokeMethodAsync<TOut>(serviceName, serviceEndpoint, httpExtension);
        }

        protected async Task<TOut> InvokeMethodAsync<TOut, TBody>(string serviceName, string serviceEndpoint,
            HTTPVerb type, IDictionary<string, string> queryParameters = null, TBody body = null) where TBody : class
        {
            var httpExtension = GenerateExtension(type, queryParameters: queryParameters);
            return await DaprClient.InvokeMethodAsync<TBody, TOut>(serviceName, serviceEndpoint, body,
                httpExtension);
        }

        protected async Task<ILookup<T, D>> GetEntitiesByReferenceIdAsLookupsAsync<T, D>(IEnumerable<T> referenceIds, 
            string serviceName, string serviceEndpoint) where T : IConvertible
        {
            var result =
                await InvokeMethodAsync<DTO.SuccessResponse<Dictionary<string, IList<D>>>, IEnumerable<T>>(serviceName,
                    serviceEndpoint, HTTPVerb.Post, body: referenceIds);
            return result?.Content
                .SelectMany(x => x.Value, Tuple.Create)
                .ToLookup(y => (T) Convert.ChangeType(y.Item1.Key, typeof(T)), y => y.Item2);
        }
        
        protected async Task<IDictionary<T, D>> GetEntitiesByReferenceIdAsDictionaryAsync<T, D>(IEnumerable<T> referenceIds,
            string serviceName, string serviceEndpoint) where T : IConvertible
        {
            var result =
                await InvokeMethodAsync<DTO.SuccessResponse<Dictionary<string, D>>, IEnumerable<T>>(serviceName,
                    serviceEndpoint, HTTPVerb.Post, body: referenceIds);
            return result?.Content.ToDictionary(key => (T) Convert.ChangeType(key.Key, typeof(T)),
                value => value.Value);
        }
    }
}