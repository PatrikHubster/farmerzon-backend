using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class CountryOutputType : ObjectGraphType<DTO.Country>
    {
        private IDataLoaderContextAccessor Accessor { get; set; }

        private void InitDependencies(IDataLoaderContextAccessor accessor)
        {
            Accessor = accessor;
        }

        private void InitType()
        {
            Name = "Country";
            
            Field<IdGraphType, long>().Name("countryId");

            Field<ListGraphType<AddressOutputType>, IEnumerable<DTO.Address>>()
                .Name("addresses")
                .ResolveAsync(LoadAddresses);

            Field<StringGraphType, string>().Name("name");
            Field<StringGraphType, string>().Name("code");
        }

        public CountryOutputType(IDataLoaderContextAccessor accessor)
        {
            InitDependencies(accessor);
            InitType();
        }
        
        private Task<IEnumerable<DTO.Address>> LoadAddresses(ResolveFieldContext<DTO.Country> context)
        {
            throw new System.NotImplementedException();
        }
    }
}