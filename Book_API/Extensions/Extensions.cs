using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Book_API.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// 動的Where条件生成
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Condition">動的条件</param>
        /// <param name="Predicate">実際の条件</param>
        /// <returns>動的Where条件</returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> Source, bool Condition, Expression<Func<TSource, bool>> Predicate)
        {
            if (Condition)
                return Source.Where(Predicate);
            else
                return Source;
        }
    }
}
