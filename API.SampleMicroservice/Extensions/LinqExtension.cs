using System.Linq.Expressions;
using System.Reflection;

namespace API.SampleMicroservice.Extensions
{
    public static class LinqExtensions
    {
        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = Array.Find(properties, p => p.Name == name);
            return matchedProperty ?? throw new ArgumentException(null, nameof(name));
        }

        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);
			return expr;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, string name)
        {
            try
            {
                var propInfo = GetPropertyInfo(typeof(T), name);
                var expr = GetOrderExpression(typeof(T), propInfo);
				bool isString = propInfo.PropertyType == typeof(string);

				var method = typeof(Enumerable).GetMethods().ToList()
                    .Find(m => m.Name == "OrderBy" && m.GetParameters().Length == (isString ? 3 : 2))
                    ?? throw new ArgumentException(null, nameof(name));
                var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);

                return (isString ? genericMethod.Invoke(null, new object[] { query, expr.Compile(), StringComparer.OrdinalIgnoreCase }) : genericMethod.Invoke(null, new object[] { query, expr.Compile() })) as IEnumerable<T> ?? query;
            }
            catch
            {
                return query;
            }

        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, Func<TSource, string> keySelector)
        {
            return source.OrderBy(keySelector, StringComparer.OrdinalIgnoreCase);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string name)
        {
            try
            {
				var propInfo = GetPropertyInfo(typeof(T), name);
				var expr = GetOrderExpression(typeof(T), propInfo);

                var method = typeof(Queryable).GetMethods().ToList()
                    .Find(m => m.Name == "OrderBy" && m.GetParameters().Length == 2)
                    ?? throw new ArgumentException(null, nameof(name));
                var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                return genericMethod.Invoke(null, new object[] { query, expr }) as IQueryable<T> ?? query;
			}
            catch
            {

                return query;
            }
        }

		public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> query, string name)
        {
            try
            {
                var propInfo = GetPropertyInfo(typeof(T), name);
                var expr = GetOrderExpression(typeof(T), propInfo);
				bool isString = propInfo.PropertyType == typeof(string);

				var method = typeof(Enumerable).GetMethods().ToList()
                    .Find(m => m.Name == "OrderByDescending" && m.GetParameters().Length == (isString ? 3 : 2))
					?? throw new ArgumentException(null, nameof(name));

				var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);

                return (isString ? genericMethod.Invoke(null, new object[] { query, expr.Compile(), StringComparer.OrdinalIgnoreCase }) : genericMethod.Invoke(null, new object[] { query, expr.Compile() })) as IEnumerable<T> ?? query;
            }
            catch
            {
                return query;
            }

        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string name)
        {
            try
            {
                var propInfo = GetPropertyInfo(typeof(T), name);
                var expr = GetOrderExpression(typeof(T), propInfo);

				var method = typeof(Queryable).GetMethods().ToList()
                    .Find(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2)
					?? throw new ArgumentException(null, nameof(name));
                var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                return genericMethod.Invoke(null, new object[] { query, expr }) as IQueryable<T> ?? query;
            }
            catch
            {
                return query;
            }
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, Func<TSource, string> keySelector)
        {
            return source.OrderByDescending(keySelector, StringComparer.OrdinalIgnoreCase);
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource>(this IOrderedEnumerable<TSource> source, Func<TSource, string> keySelector)
        {
            return source.ThenBy(keySelector, StringComparer.OrdinalIgnoreCase);
        }

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource>(this IOrderedEnumerable<TSource> source, Func<TSource, string> keySelector)
        {
            return source.ThenByDescending(keySelector, StringComparer.OrdinalIgnoreCase);
        }
    }
}
