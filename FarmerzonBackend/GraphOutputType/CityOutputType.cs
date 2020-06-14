using System.Collections.Generic;
using System.Threading.Tasks;
using FarmerzonBackendManager.Interface;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class CityOutputType : ObjectGraphType<DTO.City>
    {
        private IDataLoaderContextAccessor Accessor { get; set; }
        private IAddressManager AddressManager { get; set; }

        private void InitDependencies(IDataLoaderContextAccessor accessor, IAddressManager addressManager)
        {
            Accessor = accessor;
            AddressManager = addressManager;
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

        public CityOutputType(IDataLoaderContextAccessor accessor, IAddressManager addressManager)
        {
            InitDependencies(accessor, addressManager);
            InitType();
        }
        
        private Task<IEnumerable<DTO.Address>> LoadAddresses(ResolveFieldContext<DTO.City> context)
        {
            var loader =
                Accessor.Context.GetOrAddCollectionBatchLoader<long, DTO.Address>("GetAddressesByCityId",
                    AddressManager.GetAddressesByCityIdAsync);
            return loader.LoadAsync(context.Source.CityId);
        }
    }
}