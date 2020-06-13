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
        private IArticleManager ArticleManager { get; set; }

        private void InitDependencies(IDataLoaderContextAccessor accessor, IArticleManager articleManager)
        {
            Accessor = accessor;
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

        private Task<DTO.Address> LoadAddress(ResolveFieldContext<DTO.Person> arg)
        {
            throw new System.NotImplementedException();
        }

        public PersonOutputType(IDataLoaderContextAccessor accessor, IArticleManager articleManager)
        {
            InitDependencies(accessor, articleManager);
            InitType();
        }
        
        private Task<IEnumerable<DTO.Article>> LoadArticles(ResolveFieldContext<DTO.Person> context)
        {
            var loader =
                Accessor.Context.GetOrAddCollectionBatchLoader<long, DTO.Article>("GetArticlesByPersonId",
                    ArticleManager.GetArticlesByPersonIdAsync);
            return loader.LoadAsync(context.Source.PersonId);
        }
    }
}