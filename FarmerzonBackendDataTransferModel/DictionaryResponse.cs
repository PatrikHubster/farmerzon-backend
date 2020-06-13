using System.Collections.Generic;

namespace FarmerzonBackendDataTransferModel
{
    public class DictionaryResponse<T> : BaseResponse
    {
        public IDictionary<string, T> Content { get; set; }
    }
}