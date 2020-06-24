using System;
using System.Collections.Generic;
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
    public class CityManager : AbstractManager<DTO.City>, ICityManager
    {
        public CityManager(IHttpClientFactory clientFactory, ITokenManager tokenManager) 
            : base(clientFactory, tokenManager)
        {
            // nothing to do here
        }

        public async Task<IList<DTO.City>> GetEntitiesAsync(long? cityId, string zipCode, string name)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (cityId != null)
            {
                query.Add(nameof(cityId), cityId.Value.ToString());
            }

            if (!string.IsNullOrEmpty(zipCode))
            {
                query.Add("zipCode", zipCode);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query.Add("name", name);
            }

            var httpClient = ClientFactory.CreateClient(FarmerzonAddress);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenManager.Token);
            var builder = new UriBuilder($"{httpClient.BaseAddress}city")
            {
                Query = query.ToString() ?? string.Empty
            };
            var httpResponse = await httpClient.GetAsync(builder.ToString());

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            var cities = JsonConvert.DeserializeObject<DTO.SuccessResponse<IList<DTO.City>>>(httpResponseContent);
            return cities.Content;
        }

        public async Task<IDictionary<long, DTO.City>> GetCitiesByAddressIdAsync(IEnumerable<long> addressIds)
        { 
            return await GetEntitiesByReferenceIdAsDictAsync(addressIds, nameof(addressIds), FarmerzonAddress,
                "city/get-by-address-id");
        }
    }
}