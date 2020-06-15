using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using FarmerzonBackendManager.Interface;
using Newtonsoft.Json;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackendManager.Implementation
{
    public class AddressManager : AbstractManager<DTO.Address>, IAddressManager
    {
        public AddressManager(IHttpClientFactory clientFactory, ITokenManager tokenManager) : 
            base(clientFactory, tokenManager)
        {
            // nothing to do here
        }

        public async Task<IList<DTO.Address>> GetEntitiesAsync(long? addressId, string doorNumber, string street)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (addressId != null)
            {
                query.Add(nameof(addressId), addressId.Value.ToString());
            }

            if (!string.IsNullOrEmpty(doorNumber))
            {
                query.Add(nameof(doorNumber), doorNumber);
            }

            if (!string.IsNullOrEmpty(street))
            {
                query.Add(nameof(street), street);
            }

            var httpClient = ClientFactory.CreateClient(FarmerzonAddress);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.Token);
            var builder = new UriBuilder($"{httpClient.BaseAddress}address")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await httpClient.GetAsync(builder.ToString());

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            var addresses = JsonConvert.DeserializeObject<DTO.ListResponse<DTO.Address>>(httpResponseContent);
            return addresses.Content;
        }

        public async Task<ILookup<long, DTO.Address>> GetAddressesByCityIdAsync(IEnumerable<long> cityIds)
        {
            return await GetEntitiesByReferenceIdAsLookupAsync(cityIds, nameof(cityIds), FarmerzonAddress,
                "address/get-by-city-id");
        }

        public async Task<ILookup<long, DTO.Address>> GetAddressesByCountryIdAsync(IEnumerable<long> countryIds)
        {
            return await GetEntitiesByReferenceIdAsLookupAsync(countryIds, nameof(countryIds), FarmerzonAddress,
                "address/get-by-country-id");
        }

        public async Task<ILookup<long, DTO.Address>> GetAddressesByStateIdAsync(IEnumerable<long> stateIds)
        {
            return await GetEntitiesByReferenceIdAsLookupAsync(stateIds, nameof(stateIds), FarmerzonAddress,
                "address/get-by-state-id");
        }

        public async Task<IDictionary<string, DTO.Address>> GetAddressesByNormalizedUserName(IEnumerable<string> normalizedUserNames)
        {
            return await GetEntitiesByReferenceIdAsDictAsync(normalizedUserNames, nameof(normalizedUserNames),
                FarmerzonAddress,
                "address/get-by-normalized-user-name");
        }
    }
}