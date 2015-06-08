using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 配送数据统计查询对象
    /// </summary>
    public class OrderDistributionAnalyze
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 发货位置—抢单位置
        /// </summary>
        public string PubToGrabDistance
        {
            get;
            set;
        }
        /// <summary>
        /// 发货位置—抢单位置
        /// </summary>
        /// <returns></returns>
        public string[] GetPubToGrabDistance()
        {
            return this.GetData(this.PubToGrabDistance);
        }
        /// <summary>
        /// 抢单位置—送达位置
        /// </summary>
        public string GrabToComplete
        {
            get;
            set;
        }
        /// <summary>
        /// 抢单位置—送达位置
        /// </summary>
        /// <returns></returns>
        public string[] GetGrabToComplete()
        {
            return this.GetData(this.GrabToComplete);
        }
        /// <summary>
        /// 发货位置—送达位置
        /// </summary>
        public string PubToComplete
        {
            get;
            set;
        }
        /// <summary>
        /// 获得发货位置—送达位置
        /// </summary>
        /// <returns></returns>
        public string[] GetPubToComplete()
        {
            return this.GetData(this.PubToComplete);

        }
        /// <summary>
        /// 任务完成时长
        /// </summary>
        public string DateLength
        {
            get;
            set;
        }
        /// <summary>
        /// 获得任务时间长范围
        /// </summary>
        /// <returns></returns>
        public string[] GetDataRange()
        {
            return this.GetData(this.DateLength);
        }
        private string[] GetData(string s)
        {
            if (string.IsNullOrWhiteSpace(s) || s == "0")
            {
                return new string[1] { "0" };
            }
            else
            {
                return s.Split('-');
            }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate
        {
            get;
            set;
        }
        /// <summary>
        /// 筛选城市
        /// </summary>
        public string City
        {
            get;
            set;
        }
    }
}
