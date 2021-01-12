using System.Collections.Generic;
using System.Threading.Tasks;
using FarmerzonBackendManager.Interface;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class CityOutputType : ObjectGraphType<DTO.CityOutput>
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
            
            Field<IdGraphType, long>().Name("id");

            Field<ListGraphType<AddressOutputType>, IEnumerable<DTO.AddressOutput>>()
                .Name("addresses")
                .ResolveAsync(LoadAddressesAsync);

            Field<StringGraphType, string>().Name("zipCode");
            Field<StringGraphType, string>().Name("name");
        }

        public CityOutputType(IDataLoaderContextAccessor accessor, IAddressManager addressManager)
        {
            InitDependencies(accessor, addressManager);
            InitType();
        }
        
        private Task<IEnumerable<DTO.AddressOutput>> LoadAddressesAsync(ResolveFieldContext<DTO.CityOutput> context)
        {
            var loader =
                Accessor.Context.GetOrAddCollectionBatchLoader<long, DTO.AddressOutput>("GetAddressByCityIdAsync",
                    AddressManager.GetAddressesByCityIdAsync);
            return loader.LoadAsync(context.Source.Id);
        }
    }
}