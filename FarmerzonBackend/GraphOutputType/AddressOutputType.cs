using System;
using System.Threading.Tasks;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class AddressOutputType : ObjectGraphType<DTO.Address>
    {
        private IDataLoaderContextAccessor Accessor { get; set; }

        private void InitDependencies(IDataLoaderContextAccessor accessor)
        {
            Accessor = accessor;
        }

        private void InitType()
        {
            Name = "Address";
            
            Field<IdGraphType, long>().Name("addressId");

            Field<CityOutputType, DTO.City>()
                .Name("city")
                .ResolveAsync(LoadCity);
            Field<CountryOutputType, DTO.Country>()
                .Name("city")
                .ResolveAsync(LoadCountry);
            Field<StateOutputType, DTO.State>()
                .Name("city")
                .ResolveAsync(LoadState);

            Field<StringGraphType, string>().Name("doorNumber");
            Field<StringGraphType, string>().Name("street");
        }

        public AddressOutputType(IDataLoaderContextAccessor accessor)
        {
            InitDependencies(accessor);
            InitType();
        }
        
        private Task<DTO.City> LoadCity(ResolveFieldContext<DTO.Address> context)
        {
            throw new NotImplementedException();
        }
        
        private Task<DTO.State> LoadState(ResolveFieldContext<DTO.Address> context)
        {
            throw new NotImplementedException();
        }

        private Task<DTO.Country> LoadCountry(ResolveFieldContext<DTO.Address> context)
        {
            throw new NotImplementedException();
        }
    }
}