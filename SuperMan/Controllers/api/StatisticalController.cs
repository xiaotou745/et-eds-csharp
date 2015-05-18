using System.Collections.Generic;
using System.Web.Http;
using Ets.Model.Common;
using Ets.Model.DomainModel.Statistics;
using Ets.Service.IProvider.Statistics;
using Ets.Service.Provider.Statistics;

namespace SuperMan.Controllers.API
{
    public class StatisticalController : ApiController
    {
        /// <summary>
        /// 订单统计提供者
        /// </summary>
        private readonly IOrderStatisticsProvider orderProvider = new OrderStatisticsProvider();

        private readonly IStatisticsProvider statisticsProvider = new StatisticsProvider();

        /// <summary>
        /// 订单时间间隔统计
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public object QueryCompleteTimeSpan([FromBody]ParamOrderCompleteTimeSpan queryInfo)
        {
            if (queryInfo == null || !queryInfo.StartDate.HasValue || !queryInfo.EndDate.HasValue)
            {
                return Json(new ResultModel(false, "时间条件不允许为null", null));
            }

            if ((queryInfo.EndDate.Value.AddDays(1) - queryInfo.StartDate.Value).Days > 7)
            {
                return Json(new ResultModel(false, "最多查询7天数据，请更改查询条件", null));
            }

            IList<OrderCompleteTimeSpanInfo> lstOrderTimeSpans = orderProvider.QueryOrderCompleteTimeSpan(queryInfo);

            return new ResultModel(true, string.Empty, lstOrderTimeSpans);
        }

        [HttpPost]
        public object QueryActive([FromBody] ParamActiveInfo queryInfo)
        {
            if (queryInfo == null || !queryInfo.StartDate.HasValue || !queryInfo.EndDate.HasValue)
            {
                return Json(new ResultModel(false, "时间条件不允许为null", null));
            }

            if ((queryInfo.EndDate.Value.AddDays(1) - queryInfo.StartDate.Value).Days > 7)
            {
                return Json(new ResultModel(false, "最多查询7天数据，请更改查询条件", null));
            }

            var lstActives = statisticsProvider.QueryActiveBusinessClienter(queryInfo);
            return new ResultModel(true, string.Empty, lstActives);
        }
    }
}
