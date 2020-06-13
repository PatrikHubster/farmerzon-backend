using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class CityOutputType : ObjectGraphType<DTO.City>
    {
        private IDataLoaderContextAccessor Accessor { get; set; }

        private void InitDependencies(IDataLoaderContextAccessor accessor)
        {
            Accessor = accessor;
        }

        private void InitType()
        {
            Name = "City";
            
            Field<IdGraphType, long>().Name("cityId");

            Field<ListGraphType<AddressOutputType>, IEnumerable<DTO.Address>>()
                .Name("addresses")
                .ResolveAsync(LoadAddresses);

            Field<StringGraphType, string>().Name("zipCode");
            Field<StringGraphType, string>().Name("name");
        }

        public CityOutputType(IDataLoaderContextAccessor accessor)
        {
            InitDependencies(accessor);
            InitType();
        }
        
        private Task<IEnumerable<DTO.Address>> LoadAddresses(ResolveFieldContext<DTO.City> context)
        {
            throw new System.NotImplementedException();
        }
    }
}