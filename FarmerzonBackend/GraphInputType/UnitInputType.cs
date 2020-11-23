using GraphQL.Types;

using DTO = FarmerzonBackendDataTransferModel;

namespace FarmerzonBackend.GraphInputType
{
    public class UnitInputType : InputObjectGraphType<DTO.UnitInput>
    {
        public void InitType()
        {
            Name = "Unit";
            Field<NonNullGraphType<StringGraphType>, string>().Name("name");
        }

        public UnitInputType()
        {
            InitType();
        }
    }
}