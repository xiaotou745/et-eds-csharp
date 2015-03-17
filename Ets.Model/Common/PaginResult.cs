using System;

namespace Ets.Model.Common
{
    /// <summary>
    /// 最新分页请求实体 平扬2015.3.17
    /// </summary>
    public class NewPagingResult
    {
        private int _pageIndex;

        public NewPagingResult()
        {

        }
        public NewPagingResult(int pageIndex, int pageSize)
        {
            this._pageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        public int PageIndex
        {
            get
            {
                return _pageIndex == 0 ? 1 : _pageIndex;
            }
            set { _pageIndex = value; }
        }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
        public int RecordCount { get; set; }

        public int TotalPages
        {
            get
            {
                var val = (float)TotalCount / PageSize;
                var count = (int)Math.Ceiling(val);
                return count;
            }
        }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }

        public bool HasNextPage
        {
            get { return (PageIndex < TotalPages); }
        }
    }

    public class PagingResult
    {
        public PagingResult()
        {

        }
        public PagingResult(int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int TotalCount { get; set; }
        public int RecordCount { get; set; }

        public int TotalPages
        {
            get
            {
                var val = (float)TotalCount / PageSize;
                var count = (int)Math.Ceiling(val);
                return count;
            }
        }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }

        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }
    }
}
