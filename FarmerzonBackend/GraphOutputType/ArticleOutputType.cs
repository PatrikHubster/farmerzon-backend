using System;
using System.Threading.Tasks;
using FarmerzonBackendManager.Interface;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class ArticleOutputType : ObjectGraphType<DTO.Article>
    {
        private IDataLoaderContextAccessor Accessor { get; set; }
        private IPersonManager PersonManager { get; set; }
        private IUnitManager UnitManager { get; set; }

        private void InitDependencies(IDataLoaderContextAccessor accessor, IPersonManager personManager,
            IUnitManager unitManager)
        {
            Accessor = accessor;
            PersonManager = personManager;
            UnitManager = unitManager;
        }

        private void InitType()
        {
            Name = "Article";
            
            Field<IdGraphType, long>().Name("articleId");
            
            Field<PersonOutputType, DTO.Person>()
                .Name("person")
                .ResolveAsync(LoadPerson);
            Field<UnitOutputType, DTO.Unit>()
                .Name("unit")
                .ResolveAsync(LoadUnit);
            
            Field<StringGraphType, string>().Name("name");
            Field<StringGraphType, string>().Name("description");
            Field<FloatGraphType, double>().Name("price");
            Field<FloatGraphType, double>().Name("size");
            Field<IntGraphType, int>().Name("amount");
            Field<DateTimeGraphType, DateTime>().Name("updatedAt");
            Field<DateTimeGraphType, DateTime>().Name("createdAt");
            Field<DateTimeGraphType, DateTime>().Name("expirationDate");
        }
        
        public ArticleOutputType(IDataLoaderContextAccessor accessor, IPersonManager personManager, 
            IUnitManager unitManager)
        {
            InitDependencies(accessor, personManager, unitManager);
            InitType();
        }

        private Task<DTO.Unit> LoadUnit(ResolveFieldContext<DTO.Article> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.Unit>("GetUnitByArticleId", 
                UnitManager.GetUnitsByArticleIdAsync);
            return loader.LoadAsync(context.Source.ArticleId);
        }

        private Task<DTO.Person> LoadPerson(ResolveFieldContext<DTO.Article> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.Person>("GetPersonByArticleId", 
                PersonManager.GetPeopleByArticleIdAsync);
            return loader.LoadAsync(context.Source.ArticleId);
        }
    }
}