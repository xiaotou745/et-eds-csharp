using System.Collections.Generic;

namespace ETS.Page
{
	/// <summary>
	/// 分页器接口
	/// </summary>
	/// <typeparam name="T">IPagedList<T></typeparam>
	public interface IPagedList<T>
	{
		///<summary>
		/// 记录列表
		///</summary>
		List<T> ContentList { get; }

		/// <summary>
		/// 总记录数
		/// </summary>
		int RecordCount { get; }

		/// <summary>
		/// 总页数
		/// </summary>
		int PageCount { get; }
        /// <summary>
        /// 页大小，每页20条等
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        int CurrentPage { get; set; }
         
	}
    public interface PricIPageList<T> : IPagedList<T>
        
    {
        decimal PriceCount { get; set; }
        /// <summary>
        /// 计算为1的个数
        /// </summary>
        int DefCount { get; set; }
       
       
        
    }
}