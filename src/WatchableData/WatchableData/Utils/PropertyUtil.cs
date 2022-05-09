using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WatchableData.Utils
{
    public static class PropertyUtil
    {
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            if (!(propertyLambda.Body is MemberExpression expression))
            {
                throw new ArgumentException(string.Format($"Expression '{propertyLambda}' refere-se a um método, não a uma propriedade."));
            }

            var propInfo = expression.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format($"Expression '{propertyLambda}' refere-se a um campo, não a uma propriedade."));
            }
            return propInfo;
        }
    }
}
