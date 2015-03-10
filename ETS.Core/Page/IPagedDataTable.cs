using System.Data;

namespace ETS.Page
{
    ///<summary>
    /// 分页器接口(DataTable)
    ///</summary>
    public interface IPagedDataTable
    {
        ///<summary>
        /// 数据DataTable
        ///</summary>
        DataTable ContentData { get; }

        /// <summary>
        /// 总记录数
        /// </summary>
        int RecordCount { get; }

        /// <summary>
        /// 总页数
        /// </summary>
        int PageCount { get; }
		
        /// <summary>
        /// 总变化数量
        /// </summary>
        int TotalQuantity { get; }
    }
}