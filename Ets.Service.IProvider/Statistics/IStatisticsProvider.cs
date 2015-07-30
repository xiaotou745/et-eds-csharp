using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DomainModel.Statistics;
using ETS.Data.PageData;

namespace Ets.Service.IProvider.Statistics
{ 
    public interface IStatisticsProvider
    {
        /// <summary>
        /// 执行统计数据
        /// 窦海超
        /// 2015年3月26日 15:25:55
        /// </summary>
        void ExecStatistics();
        /// <summary>
        /// 活跃商家及骑士数量统计
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        IList<ActiveBusinessClienterInfo> QueryActiveBusinessClienter(ParamActiveInfo queryInfo);
        /// <summary>
        ///  查询分页后的商家成功充值的记录信息
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        PageInfo<BusinessBalanceInfo> QueryBusinessBalance(BussinessBalanceQuery queryInfo);
        /// <summary>
        /// 查询给定条件下商家充值总金额
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        decimal QueryBusinessTotalAmount(BussinessBalanceQuery queryInfo);

        /// <summary>
        /// 查询给定条件下充值商户的个数
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        long QueryBusinessNum(BussinessBalanceQuery queryInfo);

        /// <summary>
        /// 获得App启动热力图
        /// </summary>
        /// <param name="userType">用户类型   1商家   2骑士</param>
        /// <param name="cityName">城市名</param>
        /// <param name="deliveryCompanyInfo">骑士所属物流公司,用户类型是骑士时此参数才有效</param>
        /// <returns></returns>
        IList<AppActiveInfo> GetAppActiveInfos(byte userType, string cityName, string deliveryCompanyInfo);
    }
}
