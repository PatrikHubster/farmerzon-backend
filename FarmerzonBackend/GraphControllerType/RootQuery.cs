using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FarmerzonBackend.GraphOutputType;
using FarmerzonBackendManager.Interface;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphControllerType
{
    public class RootQuery : ObjectGraphType
    {
        private IArticleManager ArticleManager { get; set; }
        private IPersonManager PersonManager { get; set; }
        private IUnitManager UnitManager { get; set; }

        private void InitDependencies(IArticleManager articleManager, IPersonManager personManager, 
            IUnitManager unitManager)
        {
            ArticleManager = articleManager;
            PersonManager = personManager;
            UnitManager = unitManager;
        }

        private void InitQuery()
        {
            Name = "RootQuery";
            Field<ListGraphType<ArticleOutputType>>(name: "articles",
                arguments: new QueryArguments
                {
                    new QueryArgument<IdGraphType> {Name = "articleId"},
                    new QueryArgument<StringGraphType> {Name = "name"},
                    new QueryArgument<StringGraphType> {Name = "description"},
                    new QueryArgument<FloatGraphType> {Name = "price"},
                    new QueryArgument<IntGraphType> {Name = "amount"},
                    new QueryArgument<FloatGraphType> {Name = "size"},
                    new QueryArgument<DateTimeGraphType> {Name = "createdAt"},
                    new QueryArgument<DateTimeGraphType> {Name = "updatedAt"}
                }, resolve: LoadArticles);

            Field<ListGraphType<PersonOutputType>>(name: "people",
                arguments: new QueryArguments
                {
                    new QueryArgument<IdGraphType> {Name = "personId"},
                    new QueryArgument<StringGraphType> {Name = "userName"},
                    new QueryArgument<StringGraphType> {Name = "normalizedUserName"}
                }, resolve: LoadPeople);

            Field<ListGraphType<UnitOutputType>>(name: "units",
                arguments: new QueryArguments
                {
                    new QueryArgument<IdGraphType> {Name = "unitId"},
                    new QueryArgument<StringGraphType> {Name = "name"}
                }, resolve: LoadUnits);
        }

        public RootQuery(IArticleManager articleManager, IPersonManager personManager, IUnitManager unitManager)
        {
            InitDependencies(articleManager, personManager, unitManager);
            InitQuery();
        }

        private async Task<IList<DTO.Article>> LoadArticles(ResolveFieldContext<object> context)
        {
            var token = context.UserContext is Dictionary<string, string> userContext 
                        && userContext.ContainsKey("token") ? userContext["token"] : null;
            var articleId = context.GetArgument<long?>("articleId");
            var name = context.GetArgument<string>("name");
            var description = context.GetArgument<string>("description");
            var price = context.GetArgument<double?>("price");
            var amount = context.GetArgument<int?>("amount");
            var size = context.GetArgument<double?>("size");
            var createdAt = context.GetArgument<DateTime?>("createdAt");
            var updatedAt = context.GetArgument<DateTime?>("updatedAt");
            return await ArticleManager.GetEntitiesAsync(articleId, name, description, price, amount, size, 
                createdAt, updatedAt);
        }
        
        private async Task<IList<DTO.Person>> LoadPeople(ResolveFieldContext<object> context)
        {
            var token = context.UserContext is Dictionary<string, string> userContext 
                        && userContext.ContainsKey("token") ? userContext["token"] : null;
            var personId = context.GetArgument<long?>("personId");
            var userName = context.GetArgument<string>("userName");
            var normalizedUserName = context.GetArgument<string>("normalizedUserName");
            return await PersonManager.GetEntitiesAsync(personId, userName, normalizedUserName);
        }
        
        private async Task<IList<DTO.Unit>> LoadUnits(ResolveFieldContext<object> context)
        {
            var token = context.UserContext is Dictionary<string, string> userContext 
                        && userContext.ContainsKey("token") ? userContext["token"] : null;
            var unitId = context.GetArgument<long?>("unitId");
            var name = context.GetArgument<string>("name");
            return await UnitManager.GetEntitiesAsync(unitId, name);
        }
    }
}