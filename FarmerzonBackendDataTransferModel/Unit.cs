using System.Collections.Generic;

namespace FarmerzonBackendDataTransferModel
{
    public class Unit
    {
        public long UnitId { get; set; }
        
        public IList<Article> Articles { get; set; }
        
        public string Name { get; set; }
    }
}