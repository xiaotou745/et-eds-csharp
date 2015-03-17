using System.Data;
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
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Bussiness;


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
            #region where
            string whereStr = "1=1 ";  //where查询条件实体类
            if (paraModel.userId != null)  //订单商户id
                whereStr += " and o.businessId=" + paraModel.userId.ToString();
            //订单状态
            if (paraModel.Status != null)
            {
                if (paraModel.Status == 4)
                {
                    whereStr += " and (o.Status = " + OrderConst.ORDER_NEW  + " or o.Status=" + OrderConst.ORDER_ACCEPT + ")";
                }
                else
                {
                    whereStr += " and o.Status=" + paraModel.Status.ToString();
                }
            }

            #endregion

            string orderByColumn = "o.id ";  //排序条件
            string columnList = @"
                                    CONVERT(VARCHAR(5),o.ActualDoneDate,108) AS ActualDoneDate,
                                    o.Amount,
                                    o.IsPay,
                                    o.OrderNo,
                                    o.PickUpAddress,
                                    CONVERT(VARCHAR(5),o.PubDate,108) AS PubDate,
                                    o.ReceviceAddress,
                                    o.ReceviceName,
                                    o.RecevicePhoneNo,
                                    o.Remark,
                                    o.Status,
                                    o.ReceviceLongitude,
                                    o.ReceviceLatitude,
                                    b.Id as BusinessId,
                                    b.Name as BusinessName,
                                    b.Longitude,
                                    b.Latitude,
                                    b.Name as PickUpName,
                                    c.TrueName as SuperManName,
                                    c.PhoneNo as SuperManPhone ";
            string tableList = @" [order](nolock) as o 
                                   LEFT join business(nolock) as b on o.businessId=b.Id
                                   LEFT join clienter(nolock) as c on o.clienterId=c.Id ";  //表名

            return new PageHelper().GetPages<T>(SuperMan_Read, paraModel.PagingResult.PageIndex, whereStr, orderByColumn, columnList, tableList, paraModel.PagingResult.PageSize, true);
        }


        /// <summary>
        /// 商户结算列表--2015.3.12 平扬
        /// </summary>
        /// <param name="t1">开始计算日期</param>
        /// <param name="t2">结束日期</param>
        /// <param name="name">商户姓名</param>
        /// <returns></returns>
        public IList<BusinessCommissionModel> GetBusinessCommission(DateTime t1, DateTime t2, string name, int groupid)
        {
            IList<BusinessCommissionModel> list = new List<BusinessCommissionModel>();
            try
            {
                string sql = " select BB.id,BB.Name,T.Amount,T.OrderCount,isnull(BB.BusinessCommission,0) BusinessCommission, CAST(isnull(BB.BusinessCommission,0) * T.Amount*0.01 as decimal(5,2)) as TotalAmount,@t1 as T1,@t2 as T2 " +
                         " from business BB with(nolock) inner join " +
                         " (select B.id,B.Name,sum(O.Amount) as Amount,sum(ISNULL(O.OrderCount,1)) as OrderCount " +
                         " from dbo.[order] O with(nolock) inner join dbo.business B with(nolock) on O.businessId=B.Id " +
                        " where O.[Status]=1  {0}  group by B.Id,B.Name) as T on BB.Id=T.Id";
                string where = " and DATEDIFF(s, O.ActualDoneDate,@t2)>1 and DATEDIFF(s, @t1,O.ActualDoneDate)>1 ";


                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("groupid", groupid);
                dbParameters.AddWithValue("t1", t1);
                dbParameters.AddWithValue("t2", t2);
                if (!string.IsNullOrEmpty(name))
                {
                    where += " and Name=@name ";
                    dbParameters.AddWithValue("name", name);
                }
                if (groupid != 0)
                {
                    where += " and groupid=@groupid";
                    dbParameters.AddWithValue("groupid", groupid);
                }

                sql = string.Format(sql, where);
                DataTable dt = DbHelper.ExecuteDataset(Config.SuperMan_Read, sql, dbParameters).Tables[0];
                list = ConvertDataTableList<BusinessCommissionModel>(dt);
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }


        /// <summary>
        /// 商户结算列表--2015.3.12 平扬
        /// </summary>
        /// <param name="t1">开始计算日期</param>
        /// <param name="t2">结束日期</param>
        /// <param name="name">商户姓名</param>
        /// <returns></returns>
        public IList<BusinessCommissionModel> GetBusinessCommissionById(DateTime t1, DateTime t2, string name, int groupid)
        {
            IList<BusinessCommissionModel> list = new List<BusinessCommissionModel>();
            try
            {
                string sql = " select BB.id,BB.Name,T.Amount,T.OrderCount,isnull(BB.BusinessCommission,0) BusinessCommission, CAST(isnull(BB.BusinessCommission,0) * T.Amount*0.01 as decimal(5,2)) as TotalAmount,@t1 as T1,@t2 as T2 " +
                         " from business BB with(nolock) inner join " +
                         " (select B.id,B.Name,sum(O.Amount) as Amount,sum(ISNULL(O.OrderCount,1)) as OrderCount " +
                         " from dbo.[order] O with(nolock) inner join dbo.business B with(nolock) on O.businessId=B.Id " +
                        " where O.[Status]=1  {0}  group by B.Id,B.Name) as T on BB.Id=T.Id";
                string where = " and DATEDIFF(s, O.ActualDoneDate,@t2)>1 and DATEDIFF(s, @t1,O.ActualDoneDate)>1 ";


                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("groupid", groupid);
                dbParameters.AddWithValue("t1", t1);
                dbParameters.AddWithValue("t2", t2);
                if (!string.IsNullOrEmpty(name))
                {
                    where += " and Name=@name ";
                    dbParameters.AddWithValue("name", name);
                }
                if (groupid != 0)
                {
                    where += " and groupid=@groupid";
                    dbParameters.AddWithValue("groupid", groupid);
                }

                sql = string.Format(sql, where);
                DataTable dt = DbHelper.ExecuteDataset(Config.SuperMan_Read, sql, dbParameters).Tables[0];
                list = ConvertDataTableList<BusinessCommissionModel>(dt);
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }


        /// <summary>
        /// 设置结算比例-平扬 2015.3.12
        /// </summary>
        /// <param name="id"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool setCommission(int id, decimal price)
        {
            bool reslut = false;
            try
            {
                string sql = " update business set BusinessCommission=@BusinessCommission where id=@id ";
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("BusinessCommission", price);
                dbParameters.AddWithValue("id", id);
                int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Read, sql, dbParameters);
                if (i > 0) reslut = true;
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriter(ex, "设置结算比例");
                throw;
            }
            return reslut;

        }
        /// <summary>
        /// 获取商户信息
        /// danny-20150316
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetBusinesses<T>(BusinessSearchCriteria criteria)
        {
            string columnList = @"   b.Id
                                    ,b.Name
                                    ,b.City
                                    ,b.district
                                    ,b.PhoneNo
                                    ,b.PhoneNo2
                                    ,b.Password
                                    ,b.CheckPicUrl
                                    ,b.IDCard
                                    ,b.Address
                                    ,b.Landline
                                    ,b.Longitude
                                    ,b.Latitude
                                    ,b.Status
                                    ,b.InsertTime
                                    ,b.districtId
                                    ,b.CityId
                                    ,b.GroupId
                                    ,b.OriginalBusiId
                                    ,b.ProvinceCode
                                    ,b.CityCode
                                    ,b.AreaCode
                                    ,b.Province
                                    ,b.CommissionTypeId
                                    ,b.DistribSubsidy
                                    ,b.BusinessCommission ";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(criteria.businessName))
            {
                sbSqlWhere.AppendFormat(" AND b.Name={0} ", criteria.businessName);
            }
            if (!string.IsNullOrEmpty(criteria.businessPhone))
            {
                sbSqlWhere.AppendFormat(" AND b.PhoneNo={0} ", criteria.businessPhone);
            }
            if (criteria.Status != -1)
            {
                sbSqlWhere.AppendFormat(" AND b.Status={0} ", criteria.Status);
            }
            if (criteria.BusinessCommission > 0)
            {
                sbSqlWhere.AppendFormat(" AND b.BusinessCommission={0} ", criteria.BusinessCommission);
            }
            if (criteria.GroupId != null)
            {
                sbSqlWhere.AppendFormat(" AND b.GroupId={0} ", criteria.GroupId);
            }
            string tableList = @" business  b WITH (NOLOCK)   ";
            string orderByColumn = " b.InsertTime ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PagingRequest.PageIndex+1, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PagingRequest.PageSize, true);
        }
    }
}
