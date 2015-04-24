using Ets.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Common
{
    public interface IHomeCountProvider
    {
        /// <summary>
        /// 获取首页统计数据
        /// 窦海超
        /// 2015年3月25日 14:16:25
        /// </summary>
        /// <returns></returns>
        HomeCountTitleModel GetHomeCountTitle();

        /// <summary>
        /// 获取首页统计数据的列表
        /// 窦海超
        /// 2015年3月25日 14:16:25
        /// </summary>
        /// <returns></returns>
        IList<HomeCountTitleModel> GetHomeCountTitleToList(int DayCount);

            /// <summary>
        /// 获取总统计数据
        /// 窦海超
        /// 2015年3月25日 15:33:00
        /// </summary>
        /// <returns></returns>
        HomeCountTitleModel GetHomeCountTitleToAllData();
        /// <summary>
        /// 获取当日数据统计
        /// danny-20150422
        /// </summary>
        /// <returns></returns>
        HomeCountTitleModel GetCurrentDateModel();
    }
}
