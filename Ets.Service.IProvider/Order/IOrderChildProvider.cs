﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order;
using Ets.Model.DomainModel.Order;
using Ets.Model.Common;
using Ets.Model.Common.AliPay;
namespace Ets.Service.IProvider.Order
{
    /// <summary>
    /// 子订单 业务逻辑类IOrderChildService 的摘要说明。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 18:48:39
    /// </summary>

    public interface IOrderChildProvider
    {
        /// <summary>
        /// 新增一条记录
        ///<param name="clienterFinanceAccount">要新增的对象</param>
        /// </summary>
        long Create(OrderChild orderChild);

        /// <summary>
        /// 修改一条记录
        ///<param name="clienterFinanceAccount">要修改的对象</param>
        /// </summary>
        void Modify(OrderChild orderChild);

        /// <summary>
        /// 删除一条记录
        /// <param name="id">id</param>
        /// </summary>
        void Remove(int id);

        /// <summary>
        /// 根据Id得到一个对象实体
        /// <param name="id">id</param>
        /// </summary>
        OrderChild GetById(int id);

        /// <summary>
        /// 获取子订单集合
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>    
        List<OrderChildInfo> GetByOrderId(int orderId);

        /// <summary>
        /// 查询子订单是否支付
        /// 窦海超
        /// 2015年5月17日 15:51:21
        /// </summary>
        /// <param name="orderId">主订单ID</param>
        /// <param name="childId">子订单ID</param>
        /// <returns>成功返回1，支付中未支付返回0</returns>
        ResultModel<PayStatusModel> GetPayStatus(int orderId, int childId);
    }
}
