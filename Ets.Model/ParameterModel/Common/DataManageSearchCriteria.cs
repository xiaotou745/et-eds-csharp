using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Common
{
    /// <summary>
    /// 导出数据等相关首页  查询实体
    /// </summary>
    public class DataManageSearchCriteria
    {
        private int _pageIndex;
        public int PageIndex
        {
            get
            {
                return _pageIndex == 0 ? 1 : _pageIndex;
            }
            set { _pageIndex = value; }
        }

        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 模版名称
        /// </summary>
        public string Name { get; set; }
    }
}
