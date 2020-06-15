using System;
using System.Threading.Tasks;
using FarmerzonBackendManager.Interface;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class AddressOutputType : ObjectGraphType<DTO.Address>
    {
        private IDataLoaderContextAccessor Accessor { get; set; }
        private ICityManager CityManager { get; set; }
        private ICountryManager CountryManager { get; set; }
        private IStateManager StateManager { get; set; }

        private void InitDependencies(IDataLoaderContextAccessor accessor, ICityManager cityManager, 
            ICountryManager countryManager, IStateManager stateManager)
        {
            Accessor = accessor;
            CityManager = cityManager;
            CountryManager = countryManager;
            StateManager = stateManager;
        }

        private void InitType()
        {
            Name = "Address";
            
            Field<IdGraphType, long>().Name("addressId");

            Field<CityOutputType, DTO.City>()
                .Name("city")
                .ResolveAsync(LoadCity);
            Field<CountryOutputType, DTO.Country>()
                .Name("country")
                .ResolveAsync(LoadCountry);
            Field<StateOutputType, DTO.State>()
                .Name("state")
                .ResolveAsync(LoadState);

            Field<StringGraphType, string>().Name("doorNumber");
            Field<StringGraphType, string>().Name("street");
        }

        public AddressOutputType(IDataLoaderContextAccessor accessor, ICityManager cityManager, 
            ICountryManager countryManager, IStateManager stateManager)
        {
            InitDependencies(accessor, cityManager, countryManager, stateManager);
            InitType();
        }
        
        private Task<DTO.City> LoadCity(ResolveFieldContext<DTO.Address> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.City>("GetCityByAddressId", 
                CityManager.GetCitiesByAddressIdAsync);
            return loader.LoadAsync(context.Source.AddressId);
        }

        private Task<DTO.Country> LoadCountry(ResolveFieldContext<DTO.Address> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.Country>("GetCountryByAddressId",
                CountryManager.GetCountriesByAddressIdAsync);
            return loader.LoadAsync(context.Source.AddressId);
        }
        
        private Task<DTO.State> LoadState(ResolveFieldContext<DTO.Address> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.State>("GetStateByAddressId",
                StateManager.GetStatesByAddressIdAsync);
            return loader.LoadAsync(context.Source.AddressId);
        }
    }
}