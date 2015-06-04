using Ets.Model.DataModel.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    public class BusinessBalanceRecordSerchCriteria:BusinessBalanceRecord
    {
        /// <summary>
        /// 操作时间（开始）
        /// </summary>
        public string OperateTimeStart { get; set; }
        /// <summary>
        /// 操作时间（结束）
        /// </summary>
        public string OperateTimeEnd { get; set; }

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
        /// <summary>
        /// 用户所有权限城市名称集合串
        /// </summary>
        public string AuthorityCityNameListStr { get; set; }
    }
}
