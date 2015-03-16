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
using Ets.Model.ParameterModel.Bussiness;
using Ets.Model.DataModel.Bussiness;
using ETS.Extension;


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
                    whereStr += " and (o.Status = " + OrderConst.ORDER_NEW + " or o.Status=" + OrderConst.ORDER_ACCEPT + ")";
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
        /// 检查当前商户是否存在 
        /// 窦海超
        /// 2015年3月16日 13:56:18
        /// </summary>
        /// <param name="PhoneNo">电话号码</param>
        /// <returns>是否存在,true存在，false不慧</returns>
        public bool CheckBusinessExistPhone(string PhoneNo)
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM dbo.business(NOLOCK) WHERE PhoneNo=@PhoneNo";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@PhoneNo", PhoneNo);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "检查当前商户是否存在");
                return false;
                throw;
            }
        }


        /// <summary>
        ///  新增店铺
        ///  窦海超
        ///  2015年3月16日 15:19:47
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回商铺ID</returns>
        public int InsertBusiness(RegisterInfoModel model)
        {
            string sql = @"
                           INSERT INTO dbo.business
                            ( Name ,
                              City ,
                              district ,
                              PhoneNo ,
                              PhoneNo2 ,
                              Password ,
                              CheckPicUrl ,
                              IDCard ,
                              Address ,
                              Landline ,
                              Longitude ,
                              Latitude ,
                              Status ,
                              InsertTime ,
                              districtId ,
                              CityId ,
                              GroupId ,
                              OriginalBusiId ,
                              ProvinceCode ,
                              CityCode ,
                              AreaCode ,
                              Province ,
                              CommissionTypeId ,
                              DistribSubsidy ,
                              BusinessCommission
                            )
                    VALUES  ( N'' , -- Name - nvarchar(100)
                              @City , -- City - nvarchar(100)
                              N'' , -- district - nvarchar(200)
                              @PhoneNo , -- PhoneNo - nvarchar(20)
                              N'' , -- PhoneNo2 - nvarchar(20)
                              @Password , -- Password - nvarchar(255)
                              N'' , -- CheckPicUrl - nvarchar(255)
                              N'' , -- IDCard - nvarchar(45)
                              N'' , -- Address - nvarchar(255)
                              N'' , -- Landline - nvarchar(15)
                              0.0 , -- Longitude - float
                              0.0 , -- Latitude - float
                              0 , -- Status - tinyint
                              GETDATE() , -- InsertTime - datetime
                              '0' , -- districtId - nvarchar(45)
                              @CityId , -- CityId - nvarchar(45)
                              @GroupId , -- GroupId - int
                              0 , -- OriginalBusiId - int
                              N'' , -- ProvinceCode - nvarchar(20)
                              N'' , -- CityCode - nvarchar(20)
                              N'' , -- AreaCode - nvarchar(20)
                              N'' , -- Province - nvarchar(20)
                              1 , -- CommissionTypeId - int
                              NULL , -- DistribSubsidy - numeric
                              NULL  -- BusinessCommission - decimal
                            )SELECT @@IDENTITY
                        ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@City", model.city);
            parm.AddWithValue("@Password", model.passWord);
            parm.AddWithValue("@PhoneNo", model.phoneNo);
            parm.AddWithValue("@CityId", model.CityId);
            parm.AddWithValue("@GroupId", model.GroupId);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm));
        }


        /// <summary>
        /// B端登录
        /// </summary>
        /// <param name="model">用户名称，用户密码</param>
        /// <returns>返回该用户实体</returns>
        public DataTable LoginSql(LoginModel model)
        {
            string sql = @"SELECT  top 1 
                        Id AS userId,
                        status,
                        city ,
                        districtId,
                        district,
                        Address,
                        Landline,
                        Name,
                        cityId,
                        phoneNo,
                        PhoneNo2,
                        DistribSubsidy
                        FROM business(nolock) where PhoneNo=@PhoneNo AND Password=@Password order by id desc";

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@PhoneNo", model.phoneNo);
            parm.AddWithValue("@Password", model.passWord);
            return DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
        }

        public business GetBusiness(int busiId)
        {
            business busi = new business();
            string selSql = @" SELECT  
         Id ,
         Name ,
         City ,
         district ,
         PhoneNo ,
         PhoneNo2 ,
         IDCard ,
         [Address] ,
         Landline ,
         Longitude ,
         Latitude ,
         [Status] ,
         InsertTime ,
         districtId ,
         CityId ,
         GroupId , 
         ProvinceCode ,
         CityCode ,
         AreaCode ,
         Province ,
         DistribSubsidy FROM dbo.business WITH(NOLOCK) WHERE Id = @busiId";

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@busiId", busiId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, selSql, parm));
            if (dt != null)
            {
                busi = DataTableHelper.ConvertDataTableList<business>(dt)[0];
            }
            return busi;
        }
    }
}
