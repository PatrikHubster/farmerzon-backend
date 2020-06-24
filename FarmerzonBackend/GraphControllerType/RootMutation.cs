using System.Threading.Tasks;
using FarmerzonBackend.GraphInputType;
using FarmerzonBackend.GraphOutputType;
using FarmerzonBackendManager.Interface;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphControllerType
{
    public class RootMutation : ObjectGraphType
    {
        private IArticleManager ArticleManager { get; set; }

        private void InitDependencies(IArticleManager articleManager)
        {
            ArticleManager = articleManager;
        }
        
        private void InitMutation()
        {
            Name = "RootMutation";

            Field<ArticleOutputType>("createArticle",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ArticleInputType>> {Name = "article"}
                ), resolve: AddArticle);
        }

        public RootMutation(IArticleManager articleManager)
        {
            InitDependencies(articleManager);
            InitMutation();
        }
        
        private async Task<DTO.Article> AddArticle(ResolveFieldContext<object> context)
        {
            var article = context.GetArgument<DTO.Article>("article");
            return await ArticleManager.AddArticle(article);
        }
    }
}