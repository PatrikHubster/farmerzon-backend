using System.Collections.Generic;

namespace FarmerzonArticlesDataTransferModel
{
    public class ListResponse<T> : BaseResponse
    {
        public IList<T> Content { get; set; }
    }
}