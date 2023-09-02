using HotChocolate.Configuration;
using HotChocolate.Data.Filters.Expressions;
using HotChocolate.Data.Filters;
using HotChocolate.Language;
using System.Linq.Expressions;
using System.Reflection;

namespace SpareParts.API.GraphQL
{
    // ref: https://stackoverflow.com/questions/74809250/how-to-override-the-filtering-linq-query-generated-by-hotchocolate
    // ref: https://stackoverflow.com/questions/28473786/how-to-get-expression-for-nullable-values-fields-without-converting-from-exp

    public abstract class QueryableTimeSpanOperationHandler : QueryableOperationHandlerBase
    {
        protected QueryableTimeSpanOperationHandler(InputParser inputParser) : base(inputParser)
        {
        }

        protected abstract int Operation { get; }

        public override bool CanHandle(
            ITypeCompletionContext context,
            IFilterInputTypeDefinition typeDefinition,
            IFilterFieldDefinition fieldDefinition)
        {
            return context.Type is TimeSpanOperationFilterInputType &&
                fieldDefinition is FilterOperationFieldDefinition operationField &&
                operationField.Id == Operation;
        }
    }

    public class TimeSpanGreaterThanOperationHandler : QueryableTimeSpanOperationHandler
    {
        private static readonly MethodInfo _compare = typeof(TimeSpan)
            .GetMethods()
            .First(x => x.Name == nameof(TimeSpan.Compare));

        public TimeSpanGreaterThanOperationHandler(InputParser inputParser) : base(inputParser)
        {
        }

        protected override int Operation => DefaultFilterOperations.GreaterThan;

        public override Expression HandleOperation(QueryableFilterContext context, IFilterOperationField field, IValueNode value, object? parsedValue)
        {
            if (parsedValue is TimeSpan parsedValueTimeSpan)
            {
                var propertyExpression = context.GetInstance();
                var hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                var valueExpression = Expression.Property(propertyExpression, "Value");
                var valueConstant = Expression.Constant(parsedValueTimeSpan);

                var compareToExpression = Expression.Call(_compare, valueExpression, valueConstant);
                var compareResultGreaterThanZeroExpression = FilterExpressionBuilder.GreaterThan(compareToExpression, 0);
                return Expression.AndAlso(hasValueExpression, compareResultGreaterThanZeroExpression);
            }

            throw new ArgumentException("Cannot handle invalid TimeSpan value.", nameof(parsedValue));
        }
    }

    public class TimeSpanGreaterThanOrEqualsOperationHandler : QueryableTimeSpanOperationHandler
    {
        private static readonly MethodInfo _compare = typeof(TimeSpan)
            .GetMethods()
            .First(x => x.Name == nameof(TimeSpan.Compare));

        public TimeSpanGreaterThanOrEqualsOperationHandler(InputParser inputParser) : base(inputParser)
        {
        }

        protected override int Operation => DefaultFilterOperations.GreaterThanOrEquals;

        public override Expression HandleOperation(QueryableFilterContext context, IFilterOperationField field, IValueNode value, object? parsedValue)
        {
            if (parsedValue is TimeSpan parsedValueTimeSpan)
            {
                var propertyExpression = context.GetInstance();
                var hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                var valueExpression = Expression.Property(propertyExpression, "Value");
                var valueConstant = Expression.Constant(parsedValueTimeSpan);

                var compareToExpression = Expression.Call(_compare, valueExpression, valueConstant);
                var compareResultGreaterThanZeroExpression = FilterExpressionBuilder.GreaterThanOrEqual(compareToExpression, 0);
                return Expression.AndAlso(hasValueExpression, compareResultGreaterThanZeroExpression);
            }

            throw new ArgumentException("Cannot handle invalid TimeSpan value.", nameof(parsedValue));
        }
    }

    public class TimeSpanLowerThanOperationHandler : QueryableTimeSpanOperationHandler
    {
        private static readonly MethodInfo _compare = typeof(TimeSpan)
            .GetMethods()
            .First(x => x.Name == nameof(TimeSpan.Compare));

        public TimeSpanLowerThanOperationHandler(InputParser inputParser) : base(inputParser)
        {
        }

        protected override int Operation => DefaultFilterOperations.LowerThan;

        public override Expression HandleOperation(QueryableFilterContext context, IFilterOperationField field, IValueNode value, object? parsedValue)
        {
            if (parsedValue is TimeSpan parsedValueTimeSpan)
            {
                var propertyExpression = context.GetInstance();
                var hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                var valueExpression = Expression.Property(propertyExpression, "Value");
                var valueConstant = Expression.Constant(parsedValueTimeSpan);

                var compareToExpression = Expression.Call(_compare, valueExpression, valueConstant);
                var compareResultGreaterThanZeroExpression = FilterExpressionBuilder.LowerThan(compareToExpression, 0);
                return Expression.AndAlso(hasValueExpression, compareResultGreaterThanZeroExpression);
            }

            throw new ArgumentException("Cannot handle invalid TimeSpan value.", nameof(parsedValue));
        }
    }

    public class TimeSpanLowerThanOrEqualsOperationHandler : QueryableTimeSpanOperationHandler
    {
        private static readonly MethodInfo _compare = typeof(TimeSpan)
            .GetMethods()
            .First(x => x.Name == nameof(TimeSpan.Compare));

        public TimeSpanLowerThanOrEqualsOperationHandler(InputParser inputParser) : base(inputParser)
        {
        }

        protected override int Operation => DefaultFilterOperations.LowerThanOrEquals;

        public override Expression HandleOperation(QueryableFilterContext context, IFilterOperationField field, IValueNode value, object? parsedValue)
        {
            if (parsedValue is TimeSpan parsedValueTimeSpan)
            {
                var propertyExpression = context.GetInstance();
                var hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                var valueExpression = Expression.Property(propertyExpression, "Value");
                var valueConstant = Expression.Constant(parsedValueTimeSpan);

                var compareToExpression = Expression.Call(_compare, valueExpression, valueConstant);
                var compareResultGreaterThanZeroExpression = FilterExpressionBuilder.LowerThanOrEqual(compareToExpression, 0);
                return Expression.AndAlso(hasValueExpression, compareResultGreaterThanZeroExpression);
            }

            throw new ArgumentException("Cannot handle invalid TimeSpan value.", nameof(parsedValue));
        }
    }
}
