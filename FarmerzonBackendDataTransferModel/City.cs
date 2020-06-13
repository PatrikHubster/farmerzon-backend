using System.Collections.Generic;

namespace FarmerzonBackendDataTransferModel
{
    public class City
    {
        public long CityId { get; set; }
        
        public IList<Address> Addresses { get; set; }
        
        public string ZipCode { get; set; }
        public string Name { get; set; }
    }
}