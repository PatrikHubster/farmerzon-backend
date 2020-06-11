using System;

namespace FarmerzonBackendDataTransferModel
{
    public class Article
    {
        public long ArticleId { get; set; }
        
        public Person Person { get; set; }
        public Unit Unit { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public double Size { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}