using System.Collections.Generic;

namespace FarmerzonBackendDataTransferModel
{
    public class Country
    {
        public long CountryId { get; set; }
        
        public IList<Address> Addresses { get; set; }
        
        public string Name { get; set; }
        public string Code { get; set; }
    }
}