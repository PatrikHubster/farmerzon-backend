using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface IArticleManager
    {
        public Task<IList<DTO.Article>> GetEntitiesAsync(long? articleId, string name, string description, 
            double? price, int? amount, double? size, DateTime? createdAt, DateTime? updatedAt);
        public Task<ILookup<string, DTO.Article>> GetArticlesByPersonNormalizedUserNameAsync(
            IEnumerable<string> normalizedUserNames);
        public Task<ILookup<long, DTO.Article>> GetArticlesByUnitIdAsync(IEnumerable<long> unitIds);
        public Task<DTO.Article> AddArticle(DTO.Article article);
    }
}