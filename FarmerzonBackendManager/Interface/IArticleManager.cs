using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface IArticleManager
    {
        public Task<IList<DTO.ArticleOutput>> GetEntitiesAsync(long? articleId, string name, string description,
            double? price, int? amount, double? size, DateTime? createdAt, DateTime? updatedAt,
            DateTime? expirationDate);
        public Task<ILookup<string, DTO.ArticleOutput>> GetArticlesByPersonNormalizedUserNameAsync(
            IEnumerable<string> normalizedUserNames);
        public Task<ILookup<long, DTO.ArticleOutput>> GetArticlesByUnitIdAsync(IEnumerable<long> unitIds);
        public Task<DTO.ArticleOutput> AddArticle(DTO.ArticleInput article);
    }
}