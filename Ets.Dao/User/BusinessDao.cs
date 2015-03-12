using ETS;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;

namespace Ets.Dao.User
{
    public class BusinessDao : DaoBase
    {
        /// <summary>
        /// 商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <param name="paraModel">查询条件实体</param>
        public virtual PageInfo<T> GetOrdersAppToSql<T>(Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp paraModel)
        {
            string whereStr = "1=1 ";  //where查询条件实体类
            string orderByColumn = "a.id ";  //排序条件
            string columnList = "a.* ";  //列名
            string tableList = "dbo.[order] AS a ";  //表名
            if (paraModel.userId != null)  //订单商户id
                whereStr = whereStr + " and a.businessId=" + paraModel.userId.ToString();
            if (paraModel.Status != null)  //订单状态
                whereStr = whereStr + " and a.Status=" + paraModel.Status.ToString();
            return new PageHelper().GetPages<T>(SuperMan_Read, 1, whereStr, orderByColumn, columnList, tableList, 10, true);  
        }

        /// <summary>
        /// 获取我的任务   根据状态判断是已完成任务还是我的任务
        /// </summary>
        /// <param name="paraModel">查询条件实体</param>
        public virtual void GetMyOrders(Ets.Model.ParameterModel.Bussiness.ClientOrderSearchCriteria criteria)
        {
            #region where语句拼接
            string where = " 1=1 ";
            if (criteria.userId != 0)
            {
                where += " and o.clienterId=@userId ";
            }
            if (criteria.status != null && criteria.status.Value != -1)
            {
                where += " and o.[Status]= " + criteria.status.Value;
            }
            else { 
                
            }
            #endregion
            
            string sql = @"
                           SELECT 
                            o.clienterId AS userId,
                            o.OrderNo,
                            o.OriginalOrderNo,
                            CONVERT(VARCHAR(5),o.PubDate,108) AS pubDate,
                            o.pickUpAddress,
                            o.receviceName,
                            o.receviceCity,
                            o.receviceAddress,
                            o.RecevicePhoneNo AS recevicePhone,
                            o.IsPay,
                            o.Remark,
                            o.Status,
                            o.ReceviceLongitude,
                            o.ReceviceLatitude,
                            --补贴
                            o.CommissionRate,
                            o.OrderCount,
                            o.DistribSubsidy,
                            o.WebsiteSubsidy,
                            o.Amount,
                            --补贴
                            b.Name AS businessName,
                            b.PhoneNo AS businessPhone,
                            REPLACE(b.City,'市','') AS pickUpCity,
                            b.Longitude,
                            b.Latitude,
                            '' as income,
                            '' as Amount,
                            '' AS distance,
                            '' AS distanceB2R
                             FROM dbo.[order](NOLOCK) AS o
                            LEFT JOIN dbo.business(NOLOCK) AS b ON o.businessId=b.Id
                            WHERE 
                            1=1 
                            --AND 
                            --o.clienterId=@clienterId AND 
                            --o.[Status]=@Status
                            ORDER BY o.Id DESC
                            ";


        }

    }
}
