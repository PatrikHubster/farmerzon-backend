using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using FarmerzonBackendManager.Interface;
using Microsoft.Extensions.Logging;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public class ArticleManager : AbstractManager, IArticleManager
    {
        private const string ArticlesServiceName = "farmerzon-articles";
        private const string ArticlesResource = "article";
        private const string ArticlesByNormalizedUserNameEndpoint = "get-by-normalized-user-name";
        private const string ArticlesByUnitEndpoint = "get-by-unit-id";
        
        public ArticleManager(ITokenManager tokenManager, DaprClient daprClient, 
            ILogger<ArticleManager> logger) : base(tokenManager, daprClient, logger)
        {
            // nothing to do here
        }

        public async Task<IList<DTO.ArticleOutput>> GetEntitiesAsync(long? articleId, string name, string description,
            double? price, int? amount, double? size, DateTime? createdAt, DateTime? updatedAt,
            DateTime? expirationDate)
        {
            IDictionary<string, string> queryParameters = new Dictionary<string, string>();
            if (articleId != null)
            {
                queryParameters.Add(nameof(articleId), articleId.Value.ToString());
            }

            if (!string.IsNullOrEmpty(name))
            {
                queryParameters.Add(nameof(name), name);
            }

            if (!string.IsNullOrEmpty(description))
            {
                queryParameters.Add(nameof(description), description);
            }

            if (price != null)
            {
                queryParameters.Add(nameof(price), price.Value.ToString("R"));
            }

            if (amount != null)
            {
                queryParameters.Add(nameof(amount), amount.Value.ToString());
            }

            if (size != null)
            {
                queryParameters.Add(nameof(size), size.Value.ToString("R"));
            }

            if (createdAt != null)
            {
                queryParameters.Add(nameof(createdAt),
                    createdAt.Value.ToString(CultureInfo.CurrentCulture));
            }

            if (updatedAt != null)
            {
                queryParameters.Add(nameof(updatedAt),
                    updatedAt.Value.ToString(CultureInfo.CurrentCulture));
            }

            if (expirationDate != null)
            {
                queryParameters.Add(nameof(expirationDate),
                    expirationDate.Value.ToString(CultureInfo.CurrentCulture));
            }

            var result = await InvokeMethodAsync<DTO.SuccessResponse<IList<DTO.ArticleOutput>>>(ArticlesServiceName,
                ArticlesResource, HTTPVerb.Get, queryParameters: queryParameters);
            return result?.Content;
        }

        public async Task<ILookup<string, DTO.ArticleOutput>> GetArticlesByNormalizedUserNameAsync(
            IEnumerable<string> normalizedUserNames)
        {
            return await GetEntitiesByReferenceIdAsLookupsAsync<string, DTO.ArticleOutput>(normalizedUserNames,
                ArticlesServiceName, $"{ArticlesResource}/{ArticlesByNormalizedUserNameEndpoint}");
        }

        public async Task<ILookup<long, DTO.ArticleOutput>> GetArticlesByUnitIdAsync(IEnumerable<long> unitIds)
        {
            return await GetEntitiesByReferenceIdAsLookupsAsync<long, DTO.ArticleOutput>(unitIds, ArticlesServiceName,
                $"{ArticlesResource}/{ArticlesByUnitEndpoint}");
        }
    }
}