using System;
using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphInputType
{
    public class ArticleInputType : InputObjectGraphType<DTO.ArticleInput>
    {
        private void InitType()
        {
            Name = "Article";
            Field<NonNullGraphType<UnitInputType>, DTO.UnitInput>().Name("unit");

            Field<NonNullGraphType<StringGraphType>, string>().Name("name");
            Field<NonNullGraphType<StringGraphType>, string>().Name("description");
            Field<NonNullGraphType<FloatGraphType>, double>().Name("price");
            Field<NonNullGraphType<FloatGraphType>, double>().Name("size");
            Field<NonNullGraphType<IntGraphType>, int>().Name("amount");
            Field<NonNullGraphType<DateTimeGraphType>, DateTime>().Name("expirationDate");
        }
        
        public ArticleInputType()
        {
            InitType();
        }
    }
}