using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Task.Common;
using Task.Domain.Order;
using Task.Model;
using Task.Model.Order;
using TaskPlatform.TaskInterface;
using System.Transactions;
using System.Threading;
using System.Data.SqlClient;

namespace Task.Dao.Order
{
    public class OrderDao : IOrderRepos
    {
        SQLServerDBHelper dbHelper = new SQLServerDBHelper();
        /// <summary>
        /// 获取公用配置信息
        /// danny-20150402
        /// </summary>
        /// <param name="strcon"></param>
        /// <returns></returns>
        public IList<GlobalConfigModel> GetGlobalConfigInfo(Config config, GlobalConfigModel model)
        {
            dbHelper.ConnectionString = config.ReadConnectionString;
            var list = new List<GlobalConfigModel>();
            try
            {
                string sqlstr =
                    @"select Id
                            ,KeyName
                            ,Value
                            ,LastUpdateTime
                            ,Remark
                        from GlobalConfig with(nolock)
                        where 1=1 ";
                if (!string.IsNullOrWhiteSpace(model.KeyName))
                {
                    sqlstr += " and KeyName=@KeyName ";
                }
                sqlstr += " order by Id desc ";
                SqlParameter[] parameters = { new SqlParameter("@KeyName", model.KeyName) };
                DataTable table = dbHelper.Query(sqlstr, parameters).Tables[0];
                list = DataTableToList.GetModelList<GlobalConfigModel>(table);
                return list;
            }
            catch (Exception ex)
            {
                var globalConfigModel = new GlobalConfigModel();
                globalConfigModel.ExceptionStr = ex.ToString();
                globalConfigModel.DealFlag = false;
                list.Add(globalConfigModel);
                return list;
            }
        }
        /// <summary>
        /// 获取超过配置时间未抢单的订单
        /// danny-20150402
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public IList<OrderModel> GetOverTimeOrder(Config config)
        {
            dbHelper.ConnectionString = config.ReadConnectionString;
            var list = new List<OrderModel>();
            try
            {
                string sqlstr = string.Format(@"select Id,
                                                DateDiff(MINUTE,PubDate, GetDate()) IntervalMinute
                                                from [order] with(nolock)
                                                where Status=0 AND DateDiff(MINUTE,PubDate, GetDate()) in ({0});", config.IntervalMinuteList);
                DataTable table = dbHelper.Query(sqlstr).Tables[0];
                list = DataTableToList.GetModelList<OrderModel>(table);
                return list;
            }
            catch (Exception ex)
            {
                var orderModel = new OrderModel();
                orderModel.ExceptionStr = ex.ToString();
                orderModel.DealFlag = false;
                list.Add(orderModel);
                return list;
            }
        }
        /// <summary>
        /// 调整订单佣金
        /// danny-20150402
        /// </summary>
        /// <param name="config"></param>
        /// <param name="orderList"></param>
        /// <returns></returns>
        public DealResultInfo AdjustOrderCommission(Config config, List<OrderModel> orderList)
        {
            var dealResultInfo = new DealResultInfo();
            try
            {
                for (int i = 0; i < orderList.Count; i++)
                {
                    TransactionOptions option = new TransactionOptions();
                    option.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, option))
                    {
                        if (UpdateOrderCommissionById(config, orderList[i].Id))
                        {
                            if (InsertOrderSubsidiesLog(config, orderList[i].Id))
                            {
                                dealResultInfo.DealSuccQty++;
                                dealResultInfo.SuccessId += orderList[i].Id.ToString();
                                ts.Complete();
                            }
                            else
                            {
                                dealResultInfo.FailId += orderList[i].Id.ToString();
                                dealResultInfo.DealFlag = false;
                                ts.Dispose();
                            }
                        }
                        else
                        {
                            dealResultInfo.FailId += orderList[i].Id.ToString();
                            dealResultInfo.DealFlag = false;
                            ts.Dispose();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                dealResultInfo.DealFlag = false;
                dealResultInfo.DealMsg = ex.ToString();
            }
            return dealResultInfo;
        }
        /// <summary>
        /// 修改订单佣金
        /// danny-20150402
        /// </summary>
        /// <param name="config"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool UpdateOrderCommissionById(Config config, int OrderId)
        {
            dbHelper.ConnectionString = config.WriteConnectionString;
            try
            {
                string sqlstr =
                    @"update [order] 
                      set OrderCommission+=@AdjustAmount,Adjustment+=@AdjustAmount 
                      where Id=@OrderId and Status=0;";
                SqlParameter[] parameters = { new SqlParameter("@OrderId", OrderId),
                                              new SqlParameter("@AdjustAmount", config.AdjustAmount)};
                int reg = dbHelper.ExecuteSql(sqlstr, parameters);
                return reg > 0 ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 修改订单佣金
        /// danny-20150402
        /// </summary>
        /// <param name="config"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool InsertOrderSubsidiesLog(Config config, int OrderId)
        {
            dbHelper.ConnectionString = config.WriteConnectionString;
            try
            {
                string sqlstr =
                    @"INSERT INTO OrderSubsidiesLog
                                (Price
                                ,OrderId
                                ,InsertTime
                                ,OptName
                                ,Remark)
                     VALUES
                                (@Price
                                ,@OrderId
                                ,Getdate()
                                ,'服务平台'
                                ,@Remark);";
                SqlParameter[] parameters = { new SqlParameter("@Price", config.AdjustAmount),
                                              new SqlParameter("@OrderId", OrderId),
                                              new SqlParameter("@Remark", "订单超过【"+config.IntervalMinute+"】分钟未被抢单，每单增加补贴【"+config.AdjustAmount+"】元")};
                int reg = dbHelper.ExecuteSql(sqlstr, parameters);
                return reg > 0 ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取集团信息
        /// danny-20150401
        /// </summary>
        /// <param name="strcon"></param>
        /// <returns></returns>
        public IList<GroupModel> GetGroupList(string strcon)
        {
            dbHelper.ConnectionString = strcon;
            var list = new List<GroupModel>();
            try
            {
                string sqlstr =
                    @"select   [Id]
                              ,[GroupName]
                              ,[CreateName]
                              ,[CreateTime]
                              ,[ModifyName]
                              ,[ModifyTime]
                              ,[IsValid]
                      from [group] with(nolock);";
                DataTable table = dbHelper.Query(sqlstr).Tables[0];
                list = DataTableToList.GetModelList<GroupModel>(table);
                return list;
            }
            catch (Exception ex)
            {
                var groupModel = new GroupModel();
                groupModel.ExceptionStr = ex.ToString();
                groupModel.DealFlag = false;
                list.Add(groupModel);
                return list;
            }
        }
    }
}
