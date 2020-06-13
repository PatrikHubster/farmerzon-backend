using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class StateOutputType : ObjectGraphType<DTO.State>
    {
        private IDataLoaderContextAccessor Accessor { get; set; }

        private void InitDependencies(IDataLoaderContextAccessor accessor)
        {
            Accessor = accessor;
        }

        private void InitType()
        {
            Name = "Country";
            
            Field<IdGraphType, long>().Name("stateId");

            Field<ListGraphType<AddressOutputType>, IEnumerable<DTO.Address>>()
                .Name("addresses")
                .ResolveAsync(LoadAddresses);

            Field<StringGraphType, string>().Name("name");
        }

        public StateOutputType(IDataLoaderContextAccessor accessor)
        {
            InitDependencies(accessor);
            InitType();
        }
        
        private Task<IEnumerable<DTO.Address>> LoadAddresses(ResolveFieldContext<DTO.State> context)
        {
            throw new System.NotImplementedException();
        }
    }
}