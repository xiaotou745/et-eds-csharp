using System.Collections.Generic;
using ETS.Expand;

namespace ETS.Page
{
	/// <summary>
	/// ��ҳ����
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class PagedList<T> : PricIPageList<T>
	{
		///<summary>
		///</summary>
		public PagedList() {}

		///<summary>
		///</summary>
		///<param name="contents"></param>
		///<param name="recordCount"></param>
		///<param name="pageCount"></param>
        public PagedList(IEnumerable<T> contents, int recordCount, int pageCount, decimal priceCount)
		{
            if (contents != null )
            { 
			ContentList = EnumerableExpand.ToList(contents);
            }
			RecordCount = recordCount;
			PageCount = pageCount;
            PriceCount=priceCount;
		}
        public PagedList(IEnumerable<T> contents, int recordCount, int pageCount)
            : this(contents, recordCount, pageCount,0)
        {

        }
        public PagedList(IEnumerable<T> contents, int recordCount, int pageCount, int pageSize,int currentPage,int defCount=0)
        {
            if (contents != null)
            {
                ContentList = EnumerableExpand.ToList(contents);
            }
            RecordCount = recordCount;
            PageCount = pageCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            DefCount = defCount;

        }
		#region IPagedList<T> Members

		public List<T> ContentList { get; set; }
        //�ܼ�¼��
		public int RecordCount { get; set; }
        //��ҳ��
		public int PageCount { get; set; }
        
        public decimal PriceCount { get; set; }
        /// <summary>
        /// ҳ��С��ÿҳ20����
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// ��ǰҳ
        /// </summary>
        public int CurrentPage { get; set; }
        public int DefCount { get; set; }

		#endregion
	}
}