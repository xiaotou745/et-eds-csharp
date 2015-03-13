using ETS.Data.PageData;
using System.Collections.Generic;
using Ets.Model.ParameterModel.Bussiness;
﻿using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using Ets.Model.Common;
using Ets.Model.DomainModel.Bussiness;

namespace Ets.Service.IProvider.User
{
    /// <summary>
    /// 商户业务逻辑接口 add by caoheyang 20150311
    /// </summary>
    public interface IBusinessProvider
    {
        /// <summary>
        /// 商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <returns></returns>
        IList<int> GetOrdersApp();
      
        PageInfo<T> GetOrdersApp<T>(Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp paraModel);

        /// <summary>
        /// 生成商户结算excel文件2015.3.12 平扬
        /// </summary>
        /// <returns></returns>
        string CreateExcel(BusinessCommissionModel paraModel);

        /// <summary>
        /// 设置结算比例2015.3.12 平扬
        /// </summary>
        /// <returns></returns>
        bool SetCommission(int id,decimal price);

        /// <summary>
        /// 商户结算列表--2015.3.12 平扬
        /// </summary>
        /// <param name="t1">开始计算日期</param>
        /// <param name="t2">结束日期</param>
        /// <param name="name">商户姓名</param>
        /// <returns></returns>
        ResultInfo<IList<BusinessCommissionModel>> GetBusinessCommission(DateTime t1, DateTime t2, string name,int groupid);
    }
}
