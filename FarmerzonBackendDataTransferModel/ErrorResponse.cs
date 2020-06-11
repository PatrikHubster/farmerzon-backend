using System.Collections.Generic;

namespace FarmerzonArticlesDataTransferModel
{
    public class ErrorResponse : BaseResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}