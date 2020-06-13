using System.Collections.Generic;

namespace FarmerzonBackendDataTransferModel
{
    public class State
    {
        public long StateId { get; set; }
        
        public IList<Address> Addresses { get; set; }
        
        public string Name { get; set; }
    }
}