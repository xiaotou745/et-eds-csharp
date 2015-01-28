using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCore.Paging
{
    public class PagingRequest
    {
        public PagingRequest()
        {

        }
        public PagingRequest(int index, int size)
        {
            this.PageIndex = index;
            this.PageSize = size;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
