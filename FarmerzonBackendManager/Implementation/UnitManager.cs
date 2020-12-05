using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using FarmerzonBackendManager.Interface;
using Microsoft.Extensions.Logging;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public class UnitManager : AbstractManager, IUnitManager
    {
        private const string ArticlesServiceName = "farmerzon-articles";
        private const string UnitResource = "unit";
        private const string UnitByArticleEndpoint = "get-by-article-id";
        
        public UnitManager(ITokenManager tokenManager, DaprClient daprClient, 
            ILogger<UnitManager> logger) : base(tokenManager, daprClient, logger)
        {
            // nothing to do here
        }
        
        public async Task<IList<DTO.UnitOutput>> GetEntitiesAsync(long? unitId, string name)
        {
            Dictionary<string, string> queryParameter = new Dictionary<string, string>();
            if (unitId != null)
            {
                queryParameter.Add(nameof(unitId), unitId.Value.ToString());   
            }

            if (!string.IsNullOrEmpty(name))
            {
                queryParameter.Add(nameof(name), name);
            }

            var result = await InvokeMethodAsync<DTO.SuccessResponse<IList<DTO.UnitOutput>>>(ArticlesServiceName,
                UnitResource, HTTPVerb.Get, queryParameters: queryParameter);
            return result?.Content;
        }

        public async Task<IDictionary<long, DTO.UnitOutput>> GetUnitsByArticleIdAsync(IEnumerable<long> articleIds)
        {
            return await GetEntitiesByReferenceIdAsDictionaryAsync<long, DTO.UnitOutput>(articleIds,
                ArticlesServiceName, $"{UnitResource}/{UnitByArticleEndpoint}");
        }
    }
}