using HotChocolate.Data.Filters;
using HotChocolate.Data.Filters.Expressions;

namespace SpareParts.API.GraphQL
{
    public class CustomFilteringConvention : FilterConvention
    {
        protected override void Configure(IFilterConventionDescriptor descriptor)
        {
            descriptor.AddDefaults();
            descriptor.Provider(
                new QueryableFilterProvider(
                    x => x
                        .AddFieldHandler<TimeSpanGreaterThanOperationHandler>()
                        .AddFieldHandler<TimeSpanGreaterThanOrEqualsOperationHandler>()
                        .AddFieldHandler<TimeSpanLowerThanOperationHandler>()
                        .AddFieldHandler<TimeSpanLowerThanOrEqualsOperationHandler>()
                        .AddDefaultFieldHandlers()));
        }
    }
}
