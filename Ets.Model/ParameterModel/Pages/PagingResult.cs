using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Pages
{
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
