using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using FarmerzonBackendManager.Interface;
using Microsoft.Extensions.Logging;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public class PersonManager : AbstractManager, IPersonManager
    {
        private const string PersonServiceName = "farmerzon-authentication";
        private const string AddressServiceName = "farmerzon-address";
        private const string ArticlesServiceName = "farmerzon-articles";
        private const string PersonResource = "person";
        private const string PersonByArticleEndpoint = "get-by-article-id";
        private const string PersonByAddressEndpoint = "get-by-address-id";
        
        public PersonManager(ITokenManager tokenManager, DaprClient daprClient, 
            ILogger<PersonManager> logger) : base(tokenManager, daprClient, logger)
        {
            // nothing to do here
        }

        public async Task<IList<DTO.PersonOutput>> GetEntitiesAsync(string userName, string normalizedUserName)
        {
            IDictionary<string, string> queryParameters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(normalizedUserName))
            {
                queryParameters.Add(nameof(userName), userName);
            }

            if (!string.IsNullOrEmpty(normalizedUserName))
            {
                queryParameters.Add(nameof(normalizedUserName), normalizedUserName);
            }

            var result = await InvokeMethodAsync<DTO.SuccessResponse<IList<DTO.PersonOutput>>>(PersonServiceName,
                PersonResource, HTTPVerb.Get, queryParameters: queryParameters);
            return result?.Content;
        }

        public async Task<IDictionary<long, DTO.PersonOutput>> GetPeopleByArticleIdAsync(IEnumerable<long> articleIds)
        {
            return await GetEntitiesByReferenceIdAsDictionaryAsync<long, DTO.PersonOutput>(articleIds,
                ArticlesServiceName, $"{PersonResource}/{PersonByArticleEndpoint}");
        }

        public async Task<IDictionary<long, DTO.PersonOutput>> GetPeopleByAddressIdAsync(IEnumerable<long> addressIds)
        {
            return await GetEntitiesByReferenceIdAsDictionaryAsync<long, DTO.PersonOutput>(addressIds,
                AddressServiceName, $"{PersonResource}/{PersonByAddressEndpoint}");
        }
    }
}