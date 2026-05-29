using HotChocolate.Data.Filters;
using HotChocolate.Data.Filters.Expressions;
using System.Linq.Expressions;
using SpareParts.Shared.Extensions;

namespace SpareParts.API.GraphQL
{
    internal static class TimeSpanOperationHandlerHelpers
    {
        public static Expression GetTimeSpanOperation(QueryableFilterContext context, object? parsedValue, System.Reflection.MethodInfo compare, int expressionType)
        {
            TimeSpan? timeSpanValue = null;

            if (parsedValue is TimeSpan parsedValueTimeSpan)
            {
                timeSpanValue = parsedValueTimeSpan;
            }
            if (parsedValue is string parsedValueString)
            {
                if (parsedValueString.TryConvertXmlToTimeSpan(out var parsedTimeSpan))
                {
                    timeSpanValue = parsedTimeSpan;
                }
            }

            if (timeSpanValue != null)
            {
                var propertyExpression = context.GetInstance();
                var hasValueExpression = Expression.Property(propertyExpression, "HasValue");
                var valueExpression = Expression.Property(propertyExpression, "Value");
                var valueConstant = Expression.Constant(timeSpanValue);

                var compareToExpression = Expression.Call(compare, valueExpression, valueConstant);
                Expression compareResultExpression = expressionType switch
                {
                    DefaultFilterOperations.GreaterThan => FilterExpressionBuilder.GreaterThan(compareToExpression, 0),
                    DefaultFilterOperations.GreaterThanOrEquals => FilterExpressionBuilder.GreaterThanOrEqual(compareToExpression, 0),
                    DefaultFilterOperations.LowerThan => FilterExpressionBuilder.LowerThan(compareToExpression, 0),
                    DefaultFilterOperations.LowerThanOrEquals => FilterExpressionBuilder.LowerThanOrEqual(compareToExpression, 0),
                    _ => throw new ArgumentOutOfRangeException(nameof(expressionType), expressionType, null),
                };
                return Expression.AndAlso(hasValueExpression, compareResultExpression);
            }

            throw new ArgumentException("Cannot handle invalid TimeSpan value.", nameof(parsedValue));
        }
    }
}
