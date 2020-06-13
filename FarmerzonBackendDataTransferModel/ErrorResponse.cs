using System.Collections.Generic;

namespace FarmerzonBackendDataTransferModel
{
    public class ErrorResponse : BaseResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}