using System;

namespace FarmerzonBackendDataTransferModel
{
    public class ArticleInput
    {
        // relationships
        public UnitInput Unit { get; set; }
        
        // attributes
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public double Size { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}