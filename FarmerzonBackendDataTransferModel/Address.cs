namespace FarmerzonBackendDataTransferModel
{
    public class Address
    {
        public long AddressId { get; set; }
        
        public City City { get; set; }
        public Country Country { get; set; }
        public State State { get; set; }
        
        public string DoorNumber { get; set; }
        public string Street { get; set; }
    }
}