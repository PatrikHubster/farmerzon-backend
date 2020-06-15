using System.Collections.Generic;
using System.Threading.Tasks;
using FarmerzonBackendManager.Interface;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class PersonOutputType : ObjectGraphType<DTO.Person>
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
            Field<IdGraphType, long>().Name("personId");

            Field<AddressOutputType, DTO.Address>()
                .Name("address")
                .ResolveAsync(LoadAddress);
            Field<ListGraphType<ArticleOutputType>, IEnumerable<DTO.Article>>()
                .Name("articles")
                .ResolveAsync(LoadArticles);
            
            Field<StringGraphType, string>().Name("normalizedUserName");
            Field<StringGraphType, string>().Name("userName");
        }

        public PersonOutputType(IDataLoaderContextAccessor accessor, IAddressManager addressManager,
            IArticleManager articleManager)
        {
            InitDependencies(accessor, addressManager, articleManager);
            InitType();
        }
        
        private Task<DTO.Address> LoadAddress(ResolveFieldContext<DTO.Person> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<string, DTO.Address>("GetAddressByNormalizedUserName",
                AddressManager.GetAddressesByNormalizedUserName);
            return loader.LoadAsync(context.Source.NormalizedUserName);
        }
        
        private Task<IEnumerable<DTO.Article>> LoadArticles(ResolveFieldContext<DTO.Person> context)
        {
            var loader = 
                Accessor.Context.GetOrAddCollectionBatchLoader<string, DTO.Article>("GetArticlesByNormalizedUserName",
                    ArticleManager.GetArticlesByPersonNormalizedUserNameAsync);
            return loader.LoadAsync(context.Source.NormalizedUserName);
        }
    }
}