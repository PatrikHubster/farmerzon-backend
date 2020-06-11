using FarmerzonBackend.GraphInputType;
using GraphQL.Types;

namespace FarmerzonBackend.GraphControllerType
{
    public class RootMutation : ObjectGraphType
    {
        public RootMutation()
        {
            Name = "RootMutation";
            Field<UnitInputType>(
                "createUnit",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UnitInputType>> {Name = "unit"}
                ));
        }
    }
}