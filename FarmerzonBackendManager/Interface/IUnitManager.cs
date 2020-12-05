using System.Collections.Generic;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface IUnitManager
    {
        public Task<IList<DTO.UnitOutput>> GetEntitiesAsync(long? unitId, string name);
        public Task<IDictionary<long, DTO.UnitOutput>> GetUnitsByArticleIdAsync(IEnumerable<long> articleIds);
    }
}