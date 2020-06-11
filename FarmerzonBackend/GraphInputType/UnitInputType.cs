using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphInputType
{
    public class UnitInputType : InputObjectGraphType<DTO.Unit>
    {
        public UnitInputType()
        {
            Name = "Unit";
            Field<NonNullGraphType<StringGraphType>>(name: "name");
        }
    }
}