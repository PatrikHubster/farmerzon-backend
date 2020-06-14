using System.Collections.Generic;
using System.Threading.Tasks;
using FarmerzonBackendManager.Interface;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class CountryOutputType : ObjectGraphType<DTO.Country>
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
            Name = "Country";
            
            Field<IdGraphType, long>().Name("countryId");

            Field<ListGraphType<AddressOutputType>, IEnumerable<DTO.Address>>()
                .Name("addresses")
                .ResolveAsync(LoadAddresses);

            Field<StringGraphType, string>().Name("name");
            Field<StringGraphType, string>().Name("code");
        }

        public CountryOutputType(IDataLoaderContextAccessor accessor, IAddressManager addressManager)
        {
            InitDependencies(accessor, addressManager);
            InitType();
        }
        
        private Task<IEnumerable<DTO.Address>> LoadAddresses(ResolveFieldContext<DTO.Country> context)
        {
            var loader =
                Accessor.Context.GetOrAddCollectionBatchLoader<long, DTO.Address>("GetAddressesByCountryId",
                    AddressManager.GetAddressesByCountryIdAsync);
            return loader.LoadAsync(context.Source.CountryId);
        }
    }
}