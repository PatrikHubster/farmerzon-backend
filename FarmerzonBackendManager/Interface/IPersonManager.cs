using System.Collections.Generic;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface IPersonManager
    {
        public Task<IList<DTO.PersonOutput>> GetEntitiesAsync(long? personId, string userName, 
            string normalizedUserName);
        public Task<IDictionary<long, DTO.PersonOutput>> GetPeopleByArticleIdAsync(IEnumerable<long> articleIds);
    }
}