using System.Collections.Generic;

namespace FarmerzonArticlesDataTransferModel
{
    public class DictionaryResponse<T> : BaseResponse
    {
        public IDictionary<string, T> Content { get; set; }
    }
}