using System.Collections.Generic;

namespace FarmerzonBackendDataTransferModel
{
    public class ListResponse<T> : BaseResponse
    {
        public IList<T> Content { get; set; }
    }
}