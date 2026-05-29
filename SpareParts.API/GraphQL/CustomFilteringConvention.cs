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
                        .AddFieldHandler(ctx => new TimeSpanGreaterThanOperationHandler(ctx.InputParser))
                        .AddFieldHandler(ctx => new TimeSpanGreaterThanOrEqualsOperationHandler(ctx.InputParser))
                        .AddFieldHandler(ctx => new TimeSpanLowerThanOperationHandler(ctx.InputParser))
                        .AddFieldHandler(ctx => new TimeSpanLowerThanOrEqualsOperationHandler(ctx.InputParser))
                        .AddDefaultFieldHandlers()));
        }
    }
}
