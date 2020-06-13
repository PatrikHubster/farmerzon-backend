using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Interface
{
    public interface IAddressManager
    {
        public Task<IList<DTO.Address>> GetEntitiesAsync(long? addressId, string doorNumber, string street);
    }
}