using System.Threading.Tasks;
using FarmerzonBackendManager.Interface;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class AddressOutputType : ObjectGraphType<DTO.AddressOutput>
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
            
            Field<IdGraphType, long>().Name("id");

            Field<CityOutputType, DTO.CityOutput>()
                .Name("city")
                .ResolveAsync(LoadCity);
            Field<CountryOutputType, DTO.CountryOutput>()
                .Name("country")
                .ResolveAsync(LoadCountry);
            Field<StateOutputType, DTO.StateOutput>()
                .Name("state")
                .ResolveAsync(LoadState);
            Field<PersonOutputType, 

            Field<StringGraphType, string>().Name("doorNumber");
            Field<StringGraphType, string>().Name("street");
        }

        public AddressOutputType(IDataLoaderContextAccessor accessor, ICityManager cityManager, 
            ICountryManager countryManager, IStateManager stateManager)
        {
            InitDependencies(accessor, cityManager, countryManager, stateManager);
            InitType();
        }
        
        private Task<DTO.CityOutput> LoadCity(ResolveFieldContext<DTO.AddressOutput> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.CityOutput>("GetCityByAddressId", 
                CityManager.GetCitiesByAddressIdAsync);
            return loader.LoadAsync(context.Source.Id);
        }

        private Task<DTO.CountryOutput> LoadCountry(ResolveFieldContext<DTO.AddressOutput> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.CountryOutput>("GetCountryByAddressId",
                CountryManager.GetCountriesByAddressIdAsync);
            return loader.LoadAsync(context.Source.Id);
        }
        
        private Task<DTO.StateOutput> LoadState(ResolveFieldContext<DTO.AddressOutput> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.StateOutput>("GetStateByAddressId",
                StateManager.GetStatesByAddressIdAsync);
            return loader.LoadAsync(context.Source.Id);
        }
    }
}