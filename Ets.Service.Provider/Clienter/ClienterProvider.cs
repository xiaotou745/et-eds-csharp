using System.Collections.Generic;
using Ets.Dao.Clienter;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Service.IProvider.Clienter;

namespace Ets.Service.Provider.Clienter
{
    public class ClienterProvider:IClienterProvider
    {
        readonly ClienterDao clienterDao=new ClienterDao();

        public List<order> GetOrdersNoLoginLatest(ClientOrderSearchCriteria criteria)
        {
            return clienterDao.GetOrdersNoLoginLatest(criteria);
        }

        /// <summary>
        /// 骑士上下班功能 add by caoheyang 20150312
        /// </summary>
        /// <param name="paraModel"></param>
        /// <returns></returns>
        public ETS.Enums.ChangeWorkStatusEnum ChangeWorkStatus(Ets.Model.ParameterModel.Clienter.ChangeWorkStatusPM paraModel) {
            if (paraModel.WorkStatus == ETS.Const.ClienterConst.ClienterWorkStatus1)  //如果要下班，先判断超人是否还有为完成的订单
            {
                //查询当前超人有无已接单但是未完成的订单
                int ordercount = clienterDao.QueryOrderount(new Model.ParameterModel.Clienter.ChangeWorkStatusPM() { Id = paraModel.Id, OrderStatus = ETS.Const.OrderConst.OrderStatus2 });
                if (ordercount > 0) 
                    return ETS.Enums.ChangeWorkStatusEnum.OrderError;
            }
            return clienterDao.ChangeWorkStatusToSql(paraModel)>0? ETS.Enums.ChangeWorkStatusEnum.Success: ETS.Enums.ChangeWorkStatusEnum.Error;
        }
    }
}
