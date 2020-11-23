using System;
using System.Threading.Tasks;
using FarmerzonBackendManager.Interface;
using GraphQL.DataLoader;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphOutputType
{
    public class ArticleOutputType : ObjectGraphType<DTO.ArticleOutput>
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
            
            Field<IdGraphType, long>().Name("id");
            
            Field<PersonOutputType, DTO.PersonOutput>()
                .Name("person")
                .ResolveAsync(LoadPersonAsync);
            Field<UnitOutputType, DTO.UnitOutput>()
                .Name("unit")
                .ResolveAsync(LoadUnitAsync);
            
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

        private Task<DTO.UnitOutput> LoadUnitAsync(ResolveFieldContext<DTO.ArticleOutput> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.UnitOutput>("GetUnitByArticleIdAsync", 
                UnitManager.GetUnitsByArticleIdAsync);
            return loader.LoadAsync(context.Source.Id);
        }

        private Task<DTO.PersonOutput> LoadPersonAsync(ResolveFieldContext<DTO.ArticleOutput> context)
        {
            var loader = Accessor.Context.GetOrAddBatchLoader<long, DTO.PersonOutput>("GetPersonByArticleIdAsync", 
                PersonManager.GetPeopleByArticleIdAsync);
            return loader.LoadAsync(context.Source.Id);
        }
    }
}