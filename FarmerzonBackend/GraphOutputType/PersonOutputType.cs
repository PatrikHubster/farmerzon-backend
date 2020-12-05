using System.Collections.Generic;
using System.Threading.Tasks;
using FarmerzonBackendManager.Interface;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class PersonOutputType : ObjectGraphType<DTO.PersonOutput>
    {
        private IDataLoaderContextAccessor Accessor { get; set; }
        private IAddressManager AddressManager { get; set; }
        private IArticleManager ArticleManager { get; set; }

        private void InitDependencies(IDataLoaderContextAccessor accessor, IAddressManager addressManager,
            IArticleManager articleManager)
        {
            Accessor = accessor;
            AddressManager = addressManager;
            ArticleManager = articleManager;
        }

        private void InitType()
        {
            Name = "Person";

            Field<StringGraphType, string>().Name("normalizedUserName");
            
            Field<ListGraphType<AddressOutputType>, IEnumerable<DTO.AddressOutput>>()
                .Name("addresses")
                .ResolveAsync(LoadAddressesAsync);
            Field<ListGraphType<ArticleOutputType>, IEnumerable<DTO.ArticleOutput>>()
                .Name("articles")
                .ResolveAsync(LoadArticlesAsync);
            
            Field<StringGraphType, string>().Name("userName");
        }

        public PersonOutputType(IDataLoaderContextAccessor accessor, IAddressManager addressManager,
            IArticleManager articleManager)
        {
            InitDependencies(accessor, addressManager, articleManager);
            InitType();
        }
        
        private Task<IEnumerable<DTO.AddressOutput>> LoadAddressesAsync(ResolveFieldContext<DTO.PersonOutput> context)
        {
            var loader = Accessor.Context.GetOrAddCollectionBatchLoader<string, DTO.AddressOutput>(
                "GetAddressByNormalizedUserNameAsync", AddressManager.GetAddressesByNormalizedUserNameAsync);
            return loader.LoadAsync(context.Source.NormalizedUserName);
        }
        
        private Task<IEnumerable<DTO.ArticleOutput>> LoadArticlesAsync(ResolveFieldContext<DTO.PersonOutput> context)
        {
            var loader = Accessor.Context.GetOrAddCollectionBatchLoader<string, DTO.ArticleOutput>(
                "GetArticlesByNormalizedUserNameAsync", ArticleManager.GetArticlesByNormalizedUserNameAsync);
            return loader.LoadAsync(context.Source.NormalizedUserName);
        }
    }
}