using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Order;
using Ets.Dao.Order;
using Ets.Model.DomainModel.Order;
using Ets.Model.Common;
using Ets.Model.Common.AliPay;
using ETS.Enums;
using  Ets.Service.Provider.Business;
using Ets.Model.ParameterModel.Business;
using ETS.Transaction.Common;
using ETS.Transaction;
namespace Ets.Service.Provider.Order
{
    /// <summary>
    /// 订单子表 业务逻辑类IOrderChildService 的摘要说明。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 18:48:39
    /// </summary>

    public class OrderChildProvider : IOrderChildProvider
    {
        private readonly OrderChildDao _orderChildDao = new OrderChildDao();
        private readonly OrderDao orderDao = new OrderDao();
        private readonly OrderSubsidiesLogDao orderSubsidiesLogDao = new OrderSubsidiesLogDao();
        BusinessProvider businessProvider=new BusinessProvider();

        public OrderChildProvider()
        {
        }
        /// <summary>
        /// 新增一条记录
        ///<param name="clienterFinanceAccount">要新增的对象</param>
        /// </summary>
        public long Create(OrderChild orderChild)
        {
            return _orderChildDao.Insert(orderChild);
        }

        /// <summary>
        /// 修改一条记录
        ///<param name="clienterFinanceAccount">要修改的对象</param>
        /// </summary>
        public void Modify(OrderChild orderChild)
        {
            _orderChildDao.Update(orderChild);
        }

        /// <summary>
        /// 删除一条记录
        /// <param name="id">id</param>
        /// </summary>
        public void Remove(int id)
        {
            _orderChildDao.Delete(id);
        }

        /// <summary>
        /// 根据Id得到一个对象实体
        /// <param name="id">id</param>
        /// </summary>
        public OrderChild GetById(int id)
        {
            return _orderChildDao.GetById(id);
        }


        /// <summary>
        /// 获取子订单列表
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public List<OrderChildInfo> GetByOrderId(int orderId)
        {
            return _orderChildDao.GetByOrderId(orderId);
        }

        public List<OrderChild> GetListByOrderId(List<int> orderIdList)
        {
            return _orderChildDao.GetListByOrderId(orderIdList);
        }

        /// <summary>
        /// 查询子订单是否支付
        /// 窦海超
        /// 2015年5月17日 15:51:21
        /// </summary>
        /// <param name="orderId">主订单ID</param>
        /// <param name="childId">子订单ID</param>
        /// <returns>成功返回1，支付中未支付返回0</returns>
        public ResultModel<PayStatusModel> GetPayStatus(int orderId, int childId)
        {
            PayStatusModel model = _orderChildDao.GetChildPayStatus(orderId, childId);
            //if (model != null && model.PayStatus == PayStatusEnum.HadPay.GetHashCode())
            //{
            //    return ResultModel<PayResultModel>.Conclude(AliPayStatus.success);
            //}
            //return ResultModel<PayResultModel>.Conclude(AliPayStatus.fail);
            return ResultModel<PayStatusModel>.Conclude(AliPayStatus.success, model);
        }

        /// <summary>
        /// 自动取消订单
        /// </summary>
        /// 胡灵波
        /// 2015年11月19日 20:32:06
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public void AutoCancelOrder(string startTime, string endTime)
        {          
            IList<OrderChild> orderChildList = _orderChildDao.GetListByTime(startTime, endTime);
            ETS.Util.LogHelper.LogWriter("条数：" + startTime+" "+endTime+" "+orderChildList.Count.ToString());
            foreach (OrderChild item in orderChildList)
            {
                using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                {
                    long id = item.Id;//子订单id
                    int orderId = item.OrderId;//订单id
                    int orderCount = item.OrderCount;//订单数据

                    //更新子订
                    OrderChild ocModel = new OrderChild();
                    ocModel.Id = id;
                    ocModel.Status = 3;
                    ocModel.UpdateBy = "服务";
                    ocModel.UpdateTime = DateTime.Now;
                    _orderChildDao.UpdateStatus(ocModel);

                    //更新订单数量
                    order order = new order();
                    order.Id = orderId;
                    order.OrderCount = orderCount - 1;
                    orderDao.UpdateOrderCount(order);       

                    // 更新商户余额、可提现余额                        
                    businessProvider.UpdateBBalanceAndWithdraw(new BusinessMoneyPM()
                    {
                        BusinessId = item.businessId,
                        Amount = -item.SettleMoney,
                        Status = BusinessBalanceRecordStatus.Success.GetHashCode(),
                        RecordType = BusinessBalanceRecordRecordType.CancelOrder.GetHashCode(),
                        Operator = item.BusinessName,
                        WithwardId = orderId,
                        RelationNo = id.ToString(),
                        Remark = "返还配送费支出金额"
                     });

                    OrderSubsidiesLog oslModel = new OrderSubsidiesLog();
                    oslModel.OrderId = orderId;
                    oslModel.OptId = 0;
                    oslModel.OptName = "服务";
                    oslModel.Remark = "子订单id" + id.ToString();
                    oslModel.OrderStatus = ETS.Const.OrderConst.CancelOrder.GetHashCode();
                    oslModel.Platform = SuperPlatform.ServicePlatform.GetHashCode();
                    int oslId = orderSubsidiesLogDao.Insert(oslModel);

                    if(oslId>0)
                          tran.Complete();
                    }             
               }
            }    
                
    }
}
