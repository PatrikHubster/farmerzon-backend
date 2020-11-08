using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using FarmerzonBackendManager.Interface;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public class AddressManager : AbstractManager<DTO.AddressOutput>, IAddressManager
    {
        private DaprClient DaprClient { get; set; }
        
        public AddressManager(ITokenManager tokenManager, DaprClient daprClient) : base(null, tokenManager)
        {
            DaprClient = daprClient;
        }

        public async Task<IList<DTO.AddressOutput>> GetEntitiesAsync(long? addressId, string doorNumber, string street)
        {
            var httpExtension = new HTTPExtension
            {
                ContentType = "application/json", 
                Verb = HTTPVerb.Get
            };
            httpExtension.Headers.Add("Authorization", $"Bearer {TokenManager.Token}");
            
            if (addressId != null)
            {
                httpExtension.QueryString.Add(nameof(addressId), addressId.Value.ToString());
            }

            if (!string.IsNullOrEmpty(doorNumber))
            {
                httpExtension.QueryString.Add(nameof(doorNumber), doorNumber);
            }

            if (!string.IsNullOrEmpty(street))
            {
                httpExtension.QueryString.Add(nameof(street), street);
            }

            var result =
                await DaprClient.InvokeMethodAsync<DTO.SuccessResponse<IList<DTO.AddressOutput>>>("farmerzon-address",
                    "address", httpExtension);

            return result?.Content;
        }

        public async Task<ILookup<long, DTO.AddressOutput>> GetAddressesByCityIdAsync(IEnumerable<long> cityIds)
        {
            return await GetEntitiesByReferenceIdAsLookupAsync(cityIds, nameof(cityIds), FarmerzonAddress,
                "address/get-by-city-id");
        }

        public async Task<ILookup<long, DTO.AddressOutput>> GetAddressesByCountryIdAsync(IEnumerable<long> countryIds)
        {
            return await GetEntitiesByReferenceIdAsLookupAsync(countryIds, nameof(countryIds), FarmerzonAddress,
                "address/get-by-country-id");
        }

        public async Task<ILookup<long, DTO.AddressOutput>> GetAddressesByStateIdAsync(IEnumerable<long> stateIds)
        {
            return await GetEntitiesByReferenceIdAsLookupAsync(stateIds, nameof(stateIds), FarmerzonAddress,
                "address/get-by-state-id");
        }

        public async Task<IDictionary<string, DTO.AddressOutput>> GetAddressesByNormalizedUserName(IEnumerable<string> normalizedUserNames)
        {
            return await GetEntitiesByReferenceIdAsDictAsync(normalizedUserNames, nameof(normalizedUserNames),
                FarmerzonAddress,
                "address/get-by-normalized-user-name");
        }
    }
}