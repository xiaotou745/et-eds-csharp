using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    /// <summary>
    /// list页面分页查询条件 父类
    /// 曹赫洋 20150325
    /// </summary>
    public class ListParaBase
    {

        private int pageindex = ETS.Const.SystemConst.PageIndex; //默认第一页
        private int pagesize = ETS.Const.SystemConst.PageSize; //默认每页20
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get { return pageindex; }
            set { pageindex = value; }
        }


        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value; }
        }
    }
}
