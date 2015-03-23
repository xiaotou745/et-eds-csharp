using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ets.Model.Common
{
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [Serializable]
    public sealed class PagedList<T> : List<T>
    {
        public PagingResult PagingResult { get; set; }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            PagingResult = new PagingResult(pageIndex, pageSize);

            PagingResult.TotalCount = source.Count();

            var skipCount = pageIndex * pageSize;
            var data = source.Skip(skipCount).Take(pageSize).ToList();

            PagingResult.RecordCount = data.Count;

            this.AddRange(data);
        } 
    }
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [Serializable]
    public class NewPagedList<T> : List<T>
    {
        public NewPagingResult PagingResult { get; set; }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public NewPagedList(IList<T> source)
        {
            this.AddRange(source);
        }
    }
}
