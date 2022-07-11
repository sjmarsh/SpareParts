using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpareParts.Shared.Extensions
{
    public static class ExpressionExtensions
    {
        public static T GetValueFromExpression<T>(this Expression<Func<T>> expression)
        {
            var compiled = expression.Compile();
            return compiled();
        }

        public static string GetMemberNameFromExpression<TValue>(this Expression<Func<TValue>> expression)
        {
            return GetMemberInfoFromExpression(expression).Name;
        }

        public static void SetValueToExpression<TValue>(this Expression<Func<TValue>> expression, object model, TValue value)
        {
            var prop = GetMemberInfoFromExpression(expression) as PropertyInfo;
            prop?.SetValue(model, value, null);
        }

        private static MemberInfo GetMemberInfoFromExpression<TValue>(Expression<Func<TValue>> expression)
        {
            MemberExpression forMember = expression.Body switch
            {
                MemberExpression memberExpression => memberExpression,
                UnaryExpression { Operand: MemberExpression unaryMemberExpression } => unaryMemberExpression,
                _ => throw new InvalidOperationException("Must be an expression terminating in a member")
            };

            return forMember.Member;
        }
    }
}
