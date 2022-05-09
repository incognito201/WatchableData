using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WatchableData.Utils
{
    public static class PropertyExpressionExtensions
    {
        public static string GetPropertyName<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda)
        {
            return PropertyUtil.GetPropertyInfo(propertyLambda).Name;
        }

        public static MemberInfo GetMemberInfo<T>(this Expression<Func<T, object>> exp)
        {
            var member = exp.Body as MemberExpression;
            return (member ?? (exp.Body is UnaryExpression unary ? unary.Operand as MemberExpression : null)).Member;
        }
    }
}
