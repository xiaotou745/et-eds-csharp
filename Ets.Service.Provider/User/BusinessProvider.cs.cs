﻿using System.Data;
using System.Text;
using Ets.Dao.User;
using Ets.Model.Common;
using Ets.Model.DomainModel.Bussiness;
using Ets.Service.IProvider.User;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;

namespace Ets.Service.Provider.User
{

    /// <summary>
    /// 商户业务逻辑接口实现类  add by caoheyang 20150311
    /// </summary>
    public class BusinessProvider : IBusinessProvider
    {
        BusinessDao dao = new BusinessDao();
        /// <summary>
        /// app端商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual PageInfo<T> GetOrdersApp<T>(Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp paraModel)
        {
            return new BusinessDao().GetOrdersAppToSql<T>(paraModel);
        }

        /// <summary>
        /// 商户结算列表--2015.3.12 平扬
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultInfo<IList<BusinessCommissionModel>> GetBusinessCommission(DateTime t1, DateTime t2, string name,int groupid)
        { 
            var result=new ResultInfo<IList<BusinessCommissionModel>> {Data = null, Result = false, Message = ""};
            try
            {
                if (t1 > t2)
                {
                    result.Result = false;
                    result.Message = "开始时间不能大于结束时间";
                    return result;
                }
                var list = dao.GetBusinessCommission(t1, t2, name,groupid);
                if (list != null && list.Count > 0)
                {
                    result.Data = list;
                    result.Result = true;
                    result.Message = "成功";
                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "失败";
                ETS.Util.LogHelper.LogWriter(ex, "BusinessProvider.GetBusinessCommission-商户结算列表");
            }
            return result;
        }

        /// <summary>
        /// 设置结算比例2015.3.12 平扬
        /// </summary>
        /// <returns></returns>
        public bool SetCommission(int id, decimal price)
        {
            return dao.setCommission(id, price);
        }

        /// <summary>
        /// 生成商户结算excel文件2015.3.12 平扬
        /// </summary>
        /// <returns></returns>
        public string CreateExcel(BusinessCommissionModel paraModel)
        { 
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("<table border=1 cellspacing=0 cellpadding=5 rules=all>");
            //输出表头.
            strBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            strBuilder.AppendLine("<td>商户名称</td>");
            strBuilder.AppendLine("<td>订单金额</td>");
            strBuilder.AppendLine("<td>订单数量</td>");
            strBuilder.AppendLine("<td>结算比例(%)</td>");
            strBuilder.AppendLine("<td>开始时间</td>");
            strBuilder.AppendLine("<td>结束时间</td>");
            strBuilder.AppendLine("<td>结算金额</td>");
            strBuilder.AppendLine("</tr>");
            //输出数据.
            strBuilder.AppendLine(string.Format("<tr><td>{0}</td>", paraModel.Name));
            strBuilder.AppendLine(string.Format("<td>{0}</td>", paraModel.Amount));
            strBuilder.AppendLine(string.Format("<td>{0}</td>", paraModel.OrderCount));
            strBuilder.AppendLine(string.Format("<td>{0}%</td>", paraModel.BusinessCommission));
            strBuilder.AppendLine(string.Format("<td>{0}</td>", paraModel.T1.ToShortDateString()));
            strBuilder.AppendLine(string.Format("<td>{0}</td>", paraModel.T2.ToShortDateString()));
            strBuilder.AppendLine(string.Format("<td>{0}</td></tr>", paraModel.TotalAmount));
            strBuilder.AppendLine("</table>");
            return strBuilder.ToString();
        } 
        public IList<int> GetOrdersApp()
        {
            throw new NotImplementedException();
        }

        
    }
}