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
        private IAddressManager AddressManager { get; set; }
        private IArticleManager ArticleManager { get; set; }
        private ICityManager CityManager { get; set; }
        private ICountryManager CountryManager { get; set; }
        private IPersonManager PersonManager { get; set; }
        private IStateManager StateManager { get; set; }
        private IUnitManager UnitManager { get; set; }

        private void InitDependencies(IAddressManager addressManager, IArticleManager articleManager, 
            ICityManager cityManager, ICountryManager countryManager, IPersonManager personManager, 
            IStateManager stateManager, IUnitManager unitManager)
        {
            AddressManager = addressManager;
            ArticleManager = articleManager;
            CityManager = cityManager;
            CountryManager = countryManager;
            PersonManager = personManager;
            StateManager = stateManager;
            UnitManager = unitManager;
        }

        private void InitQuery()
        {
            Name = "RootQuery";
            Field<ListGraphType<AddressOutputType>>(name: "addresses",
                arguments: new QueryArguments
                {
                    new QueryArgument<IdGraphType> {Name = "id"},
                    new QueryArgument<StringGraphType> {Name = "doorNumber"},
                    new QueryArgument<StringGraphType> {Name = "street"}
                }, resolve: LoadAddresses);

            Field<ListGraphType<ArticleOutputType>>(name: "articles",
                arguments: new QueryArguments
                {
                    new QueryArgument<IdGraphType> {Name = "id"},
                    new QueryArgument<StringGraphType> {Name = "name"},
                    new QueryArgument<StringGraphType> {Name = "description"},
                    new QueryArgument<FloatGraphType> {Name = "price"},
                    new QueryArgument<IntGraphType> {Name = "amount"},
                    new QueryArgument<FloatGraphType> {Name = "size"},
                    new QueryArgument<DateTimeGraphType> {Name = "createdAt"},
                    new QueryArgument<DateTimeGraphType> {Name = "updatedAt"},
                    new QueryArgument<DateTimeGraphType> {Name = "expirationDate"}
                }, resolve: LoadArticles);

            Field<ListGraphType<CityOutputType>>(name: "cities",
                arguments: new QueryArguments
                {
                    new QueryArgument<IdGraphType> {Name = "id"},
                    new QueryArgument<StringGraphType> {Name = "zipCode"},
                    new QueryArgument<StringGraphType> {Name = "name"}
                }, resolve: LoadCities);

            Field<ListGraphType<CountryOutputType>>(name: "countries",
                arguments: new QueryArguments
                {
                    new QueryArgument<IdGraphType> {Name = "id"},
                    new QueryArgument<StringGraphType> {Name = "name"},
                    new QueryArgument<StringGraphType> {Name = "code"}
                }, resolve: LoadCountries);

            Field<ListGraphType<PersonOutputType>>(name: "people",
                arguments: new QueryArguments
                {
                    new QueryArgument<StringGraphType> {Name = "userName"},
                    new QueryArgument<StringGraphType> {Name = "normalizedUserName"}
                }, resolve: LoadPeople);

            Field<ListGraphType<StateOutputType>>(name: "states",
                arguments: new QueryArguments
                {
                    new QueryArgument<IdGraphType> {Name = "id"},
                    new QueryArgument<StringGraphType> {Name = "name"}
                }, resolve: LoadStates);

        Field<ListGraphType<UnitOutputType>>(name: "units",
                arguments: new QueryArguments
                {
                    new QueryArgument<IdGraphType> {Name = "id"},
                    new QueryArgument<StringGraphType> {Name = "name"}
                }, resolve: LoadUnits);
        }

        public RootQuery(IAddressManager addressManager, IArticleManager articleManager, 
            ICountryManager countryManager, ICityManager cityManager, IPersonManager personManager, 
            IStateManager stateManager, IUnitManager unitManager)
        {
            InitDependencies(addressManager, articleManager, cityManager, countryManager, 
                personManager, stateManager, unitManager);
            InitQuery();
        }
        
        private async Task<IList<DTO.AddressOutput>> LoadAddresses(ResolveFieldContext<object> context)
        {
            var id = context.GetArgument<long?>("id");
            var doorNumber = context.GetArgument<string>("doorNumber");
            var street = context.GetArgument<string>("street");
            return await AddressManager.GetEntitiesAsync(id, doorNumber, street);
        }

        private async Task<IList<DTO.ArticleOutput>> LoadArticles(ResolveFieldContext<object> context)
        {
            var id = context.GetArgument<long?>("id");
            var name = context.GetArgument<string>("name");
            var description = context.GetArgument<string>("description");
            var price = context.GetArgument<double?>("price");
            var amount = context.GetArgument<int?>("amount");
            var size = context.GetArgument<double?>("size");
            var createdAt = context.GetArgument<DateTime?>("createdAt");
            var updatedAt = context.GetArgument<DateTime?>("updatedAt");
            var expirationDate = context.GetArgument<DateTime?>("expirationDate");
            return await ArticleManager.GetEntitiesAsync(id, name, description, price, amount, size, 
                createdAt, updatedAt, expirationDate);
        }
        
        private async Task<IList<DTO.CityOutput>> LoadCities(ResolveFieldContext<object> context)
        {
            var id = context.GetArgument<long?>("id");
            var zipCode = context.GetArgument<string>("zipCode");
            var name = context.GetArgument<string>("name");
            return await CityManager.GetEntitiesAsync(id, zipCode, name);
        }
        
        private async Task<IList<DTO.CountryOutput>> LoadCountries(ResolveFieldContext<object> context)
        {
            var id = context.GetArgument<long?>("id");
            var name = context.GetArgument<string>("name");
            var code = context.GetArgument<string>("code");
            return await CountryManager.GetEntitiesAsync(id, name, code);
        }
        
        private async Task<IList<DTO.PersonOutput>> LoadPeople(ResolveFieldContext<object> context)
        {
            var userName = context.GetArgument<string>("userName");
            var normalizedUserName = context.GetArgument<string>("normalizedUserName");
            return await PersonManager.GetEntitiesAsync(userName, normalizedUserName);
        }
        
        private async Task<IList<DTO.StateOutput>> LoadStates(ResolveFieldContext<object> context)
        {
            var id = context.GetArgument<long?>("id");
            var name = context.GetArgument<string>("name");
            return await StateManager.GetEntitiesAsync(id, name);
        }
        
        private async Task<IList<DTO.UnitOutput>> LoadUnits(ResolveFieldContext<object> context)
        {
            var id = context.GetArgument<long?>("id");
            var name = context.GetArgument<string>("name");
            return await UnitManager.GetEntitiesAsync(id, name);
        }
    }
}