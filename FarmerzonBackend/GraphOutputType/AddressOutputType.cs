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
        private IPersonManager PersonManager { get; set; }

        private void InitDependencies(IDataLoaderContextAccessor accessor, ICityManager cityManager, 
            ICountryManager countryManager, IStateManager stateManager, IPersonManager personManager)
        {
            Accessor = accessor;
            CityManager = cityManager;
            CountryManager = countryManager;
            StateManager = stateManager;
            PersonManager = personManager;
        }

        private void InitType()
        {
            Name = "Address";
            
            Field<IdGraphType, long>().Name("id");

            Field<CityOutputType, DTO.CityOutput>()
                .Name("city")
                .ResolveAsync(LoadCityAsync);
            Field<CountryOutputType, DTO.CountryOutput>()
                .Name("country")
                .ResolveAsync(LoadCountryAsync);
            Field<StateOutputType, DTO.StateOutput>()
                .Name("state")
                .ResolveAsync(LoadStateAsync);
            Field<PersonOutputType, DTO.PersonOutput>()
                .Name("person")
                .ResolveAsync(LoadPersonAsync);

            Field<StringGraphType, string>().Name("doorNumber");
            Field<StringGraphType, string>().Name("street");
        }

        public AddressOutputType(IDataLoaderContextAccessor accessor, ICityManager cityManager, 
            ICountryManager countryManager, IStateManager stateManager, IPersonManager personManager)
        {
            InitDependencies(accessor, cityManager, countryManager, stateManager, personManager);
            InitType();
        }
        
        private Task<DTO.CityOutput> LoadCityAsync(ResolveFieldContext<DTO.AddressOutput> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.CityOutput>("GetCityByAddressIdAsync", 
                CityManager.GetCitiesByAddressIdAsync);
            return loader.LoadAsync(context.Source.Id);
        }

        private Task<DTO.CountryOutput> LoadCountryAsync(ResolveFieldContext<DTO.AddressOutput> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.CountryOutput>("GetCountryByAddressIdAsync",
                CountryManager.GetCountriesByAddressIdAsync);
            return loader.LoadAsync(context.Source.Id);
        }
        
        private Task<DTO.StateOutput> LoadStateAsync(ResolveFieldContext<DTO.AddressOutput> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.StateOutput>("GetStateByAddressIdAsync",
                StateManager.GetStatesByAddressIdAsync);
            return loader.LoadAsync(context.Source.Id);
        }

        private Task<DTO.PersonOutput> LoadPersonAsync(ResolveFieldContext<DTO.AddressOutput> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.PersonOutput>("GetPersonByAddressIdAsync",
                PersonManager.GetPeopleByAddressIdAsync);
            return loader.LoadAsync(context.Source.Id);
        }
    }
}