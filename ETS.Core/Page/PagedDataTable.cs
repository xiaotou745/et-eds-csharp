using System.Data;

namespace ETS.Page
{
    ///<summary>
    /// 
    ///</summary>
    public class PagedDataTable : IPagedDataTable
    {
        ///<summary>
        /// 
        ///</summary>
        public PagedDataTable() {}

        ///<summary>
        ///</summary>
        ///<param name="content"></param>
        ///<param name="recordCount"></param>
        ///<param name="pageCount"></param>
        public PagedDataTable(DataTable content, int recordCount, int pageCount)
        {
            ContentData = content;
            RecordCount = recordCount;
            PageCount = pageCount;
        }
        ///<summary>
        ///</summary>
        ///<param name="content"></param>
        ///<param name="recordCount"></param>
        ///<param name="pageCount"></param>
        ///<param name="totalQuantity"></param>
        public PagedDataTable(DataTable content, int recordCount, int pageCount, int totalQuantity)
        {
            ContentData = content;
            RecordCount = recordCount;
            PageCount = pageCount;
            TotalQuantity = totalQuantity;
        }
        #region IPagedDataTable Members

        public DataTable ContentData { get; set; }

        public int RecordCount { get; set; }

        public int PageCount { get; set; }

        public int TotalQuantity { get; set; }

        #endregion
    }
}