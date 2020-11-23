using System.Collections.Generic;
using System.Threading.Tasks;
using FarmerzonBackendManager.Interface;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class UnitOutputType : ObjectGraphType<DTO.UnitOutput>
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
            Name = "Unit";
            
            Field<IdGraphType, long>(name: "id");
            
            Field<ListGraphType<ArticleOutputType>, IEnumerable<DTO.ArticleOutput>>()
                .Name("articles")
                .ResolveAsync(LoadArticlesAsync);
            
            Field<StringGraphType, string>().Name("name");            
        }

        public UnitOutputType(IDataLoaderContextAccessor accessor, IArticleManager articleManager)
        {
            InitDependencies(accessor, articleManager);
            InitType();
        }
        
        private Task<IEnumerable<DTO.ArticleOutput>> LoadArticlesAsync(ResolveFieldContext<DTO.UnitOutput> context)
        {
            var loader =
                Accessor.Context.GetOrAddCollectionBatchLoader<long, DTO.ArticleOutput>("GetArticleByUnitIdAsync",
                    ArticleManager.GetArticlesByUnitIdAsync);
            return loader.LoadAsync(context.Source.Id);
        }
    }
}