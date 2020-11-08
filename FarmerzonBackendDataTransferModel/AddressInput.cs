namespace FarmerzonBackendDataTransferModel
{
    public class AddressInput
    {
        // relationships
        public CityInput City { get; set; }
        public CountryInput Country { get; set; }
        public StateInput State { get; set; }
        
        // attributes
        public string DoorNumber { get; set; }
        public string Street { get; set; }
    }
}