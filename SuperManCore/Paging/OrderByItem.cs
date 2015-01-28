using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCore.Paging
{
    public class OrderByField<TEntity>
    {
        public OrderByField(Expression<Func<TEntity, object>> keySelector, SortOrder direction)
        {
            KeySelector = keySelector;
            Direction = direction;
        }

        public Expression<Func<TEntity, object>> KeySelector { get; private set; }
        public SortOrder Direction { get; private set; }
    }

    public static class Ex
    {
        /// <summary>
        /// 此方法不调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderByFields"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, IList<OrderByField<T>> orderByFields)
        {
            var orderedQuery = query as IOrderedQueryable<T>;
            //for (int i = 0; i < orderByFields.Count; i++)
            //{
            //    var item = orderByFields[i];
            //    orderedQuery = i == 0 ? orderedQuery.ObjectSort(item.KeySelector, item.Direction) : orderedQuery.ThenObjectSort(item.KeySelector, item.Direction);
            //}
            return orderedQuery;
        }
    }
}
