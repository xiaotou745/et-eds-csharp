using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using Ets.Model.DataModel.DeliveryCompany;
using Ets.Model.ParameterModel.DeliveryCompany;
using ETS.Data.PageData;
using ETS.Util;
using ETS.Const;

namespace Ets.Dao.DeliveryCompany
{
    public class DeliveryCompanyDao : DaoBase
    {
        /// <summary>
        /// 获取物流公司列表
        /// danny-20150706
        /// </summary>
        /// <returns></returns>
        public IList<DeliveryCompanyModel> GetDeliveryCompanyList()
        {
            string sql = @"  SELECT  [Id]
                                    ,[DeliveryCompanyName]
                                    ,[DeliveryCompanyCode]
                                    ,[IsEnable]
                                    ,[SettleType]
                                    ,[ClienterFixMoney]
                                    ,[ClienterSettleRatio]
                                    ,[DeliveryCompanySettleMoney]
                                    ,[DeliveryCompanyRatio]
                                    ,[ClienterQuantity]
                            FROM [DeliveryCompany] WITH(NOLOCK)
                            WHERE IsEnable=1
                            ORDER BY Id;";
            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<DeliveryCompanyModel>(dt);
        }

        public PageInfo<T> Get<T>(DeliveryCompanyCriteria deliveryCompanyCriteria)
        {
            string columnList = @"
        Id ,
        DeliveryCompanyName ,
        DeliveryCompanyCode ,
        IsEnable ,
        SettleType ,
        ClienterFixMoney ,
        ClienterSettleRatio ,
        DeliveryCompanySettleMoney ,
        DeliveryCompanyRatio ,
        BusinessQuantity ,
        ClienterQuantity ,
        CreateTime ,
        CreateName ,
        ModifyName ,
        ModifyTime";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            if (!string.IsNullOrEmpty(deliveryCompanyCriteria.DeliveryCompanyName))
            {
                sbSqlWhere.AppendFormat(" AND DeliveryCompanyName='{0}' ", deliveryCompanyCriteria.DeliveryCompanyName.Trim());
                //dbParameters.Add("DeliveryCompanyName",DbType.String,200).Value= deliveryCompanyCriteria.DeliveryCompanyName.Trim();
            }
            string tableList = @" dbo.DeliveryCompany(nolock) ";
            string orderByColumn = " Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, deliveryCompanyCriteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, deliveryCompanyCriteria.PageSize, true);
        }

        public int Add(DeliveryCompanyModel deliveryCompanyModel)
        {
            const string insertSql = @"
insert into dbo.DeliveryCompany
         ( DeliveryCompanyName ,
           DeliveryCompanyCode ,
           SettleType ,
           ClienterFixMoney ,
           ClienterSettleRatio ,
           DeliveryCompanySettleMoney ,
           DeliveryCompanyRatio ,
           CreateName ,
           IsDisplay
         )
 output Inserted.Id,Inserted.DeliveryCompanyName,Inserted.IsEnable,Inserted.SettleType,Inserted.ClienterFixMoney,Inserted.ClienterSettleRatio,
        Inserted.DeliveryCompanySettleMoney,Inserted.BusinessQuantity,Inserted.ClienterQuantity,Inserted.IsDisplay,@CreateName,getdate()
        into dbo.DeliveryCompanyLog(DeliveryCompanyId,DeliveryCompanyName,IsEnable,SettleType,ClienterFixMoney,ClienterSettleRatio,DeliveryCompanySettleMoney,BusinessQuantity,ClienterQuantity,IsDisplay,ModifyName, ModifyTime) 
 values(
           @DeliveryCompanyName ,
           @DeliveryCompanyCode ,
           @SettleType ,
           @ClienterFixMoney ,
           @ClienterSettleRatio ,
           @DeliveryCompanySettleMoney ,
           @DeliveryCompanyRatio ,
           @CreateName ,
           @IsDisplay 
)
select @@IDENTITY ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@DeliveryCompanyName", DbType.String).Value = deliveryCompanyModel.DeliveryCompanyName;
            dbParameters.Add("@DeliveryCompanyCode", DbType.String).Value = deliveryCompanyModel.DeliveryCompanyCode;
            dbParameters.Add("@SettleType", DbType.Int16).Value = deliveryCompanyModel.SettleType;
            dbParameters.Add("@IsDisplay", DbType.Int16).Value = deliveryCompanyModel.IsDisplay;
            if (deliveryCompanyModel.SettleType == 1)
            {
                dbParameters.Add("@ClienterSettleRatio", DbType.Decimal).Value = deliveryCompanyModel.ClienterSettle;
                dbParameters.Add("@DeliveryCompanyRatio", DbType.Decimal).Value = deliveryCompanyModel.DeliveryCompanySettle;

                dbParameters.Add("@ClienterFixMoney", DbType.Decimal).Value = 0;
                dbParameters.Add("@DeliveryCompanySettleMoney", DbType.Decimal).Value = 0;
            }
            else
            {
                dbParameters.Add("@ClienterFixMoney", DbType.Decimal).Value = deliveryCompanyModel.ClienterSettle;
                dbParameters.Add("@DeliveryCompanySettleMoney", DbType.Decimal).Value = deliveryCompanyModel.DeliveryCompanySettle;

                dbParameters.Add("@ClienterSettleRatio", DbType.Decimal).Value = 0;
                dbParameters.Add("@DeliveryCompanyRatio", DbType.Decimal).Value = 0;
            }
            dbParameters.Add("@CreateName", DbType.String).Value = deliveryCompanyModel.CreateName;
            int deliveryCompanyID=ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));
            if (deliveryCompanyID>0)
            {
                UpdateRedis(deliveryCompanyID);
            }
            return deliveryCompanyID;
        }

        public int Modify(DeliveryCompanyModel deliveryCompanyModel)
        {
           StringBuilder upSql = new StringBuilder(@"
update  dbo.DeliveryCompany
 set     DeliveryCompanyName = @DeliveryCompanyName ,
        IsEnable = @IsEnable ,
        SettleType = @SettleType ,
        ModifyName = @ModifyName ,
        ModifyTime = getdate() ");

            if (deliveryCompanyModel.SettleType == 1)
            {
                deliveryCompanyModel.ClienterSettleRatio = deliveryCompanyModel.ClienterSettle;
                deliveryCompanyModel.DeliveryCompanyRatio = deliveryCompanyModel.DeliveryCompanySettle;
                upSql.Append(@" ,ClienterSettleRatio = @ClienterSettleRatio,DeliveryCompanyRatio = @DeliveryCompanyRatio ");
            }
            else
            {
                deliveryCompanyModel.ClienterFixMoney = deliveryCompanyModel.ClienterSettle;
                deliveryCompanyModel.DeliveryCompanySettleMoney = deliveryCompanyModel.DeliveryCompanySettle;
                upSql.Append(@" ,ClienterFixMoney = @ClienterFixMoney,DeliveryCompanySettleMoney = @DeliveryCompanySettleMoney ");
            }
            
            upSql.Append(@" ,IsDisplay = @IsDisplay ");
            upSql.Append(
                @" output Deleted.Id,Deleted.DeliveryCompanyName,Deleted.IsEnable,Deleted.SettleType,Deleted.ClienterFixMoney,Deleted.ClienterSettleRatio,
        Deleted.DeliveryCompanySettleMoney,Deleted.BusinessQuantity,Deleted.ClienterQuantity,Deleted.IsDisplay,@ModifyName,getdate()
        into dbo.DeliveryCompanyLog(DeliveryCompanyId,DeliveryCompanyName,IsEnable,SettleType,ClienterFixMoney,ClienterSettleRatio,DeliveryCompanySettleMoney,BusinessQuantity,ClienterQuantity,IsDisplay,ModifyName,ModifyTime) ");
            upSql.Append(@" where   Id = @Id; ");
            IDbParameters dbParameters = DbHelper.CreateDbParameters();

            dbParameters.Add("@Id", DbType.Int32).Value = deliveryCompanyModel.Id;
            dbParameters.Add("@DeliveryCompanyName", DbType.String).Value = deliveryCompanyModel.DeliveryCompanyName;
            dbParameters.Add("@SettleType", DbType.Int16).Value = deliveryCompanyModel.SettleType;
            dbParameters.Add("@IsDisplay", DbType.Int16).Value = deliveryCompanyModel.IsDisplay;
            dbParameters.Add("@IsEnable", DbType.Int16).Value = deliveryCompanyModel.IsEnable;
            if (deliveryCompanyModel.SettleType == 1)
            {
                dbParameters.Add("@ClienterSettleRatio", DbType.Decimal).Value = deliveryCompanyModel.ClienterSettle;
                dbParameters.Add("@DeliveryCompanyRatio", DbType.Decimal).Value = deliveryCompanyModel.DeliveryCompanySettle;

                //dbParameters.Add("@ClienterFixMoney", DbType.Decimal).Value = 0;
                //dbParameters.Add("@DeliveryCompanySettleMoney", DbType.Decimal).Value = 0;
            }
            else
            {
                dbParameters.Add("@ClienterFixMoney", DbType.Decimal).Value = deliveryCompanyModel.ClienterSettle;
                dbParameters.Add("@DeliveryCompanySettleMoney", DbType.Decimal).Value = deliveryCompanyModel.DeliveryCompanySettle;

                //dbParameters.Add("@ClienterSettleRatio", DbType.Decimal).Value = 0;
                //dbParameters.Add("@DeliveryCompanyRatio", DbType.Decimal).Value = 0;
            }
            dbParameters.Add("@ModifyName", DbType.String).Value = deliveryCompanyModel.ModifyName;
            int result= ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, upSql.ToString(), dbParameters));
            if (result > 0)
            {
                UpdateRedis(deliveryCompanyModel.Id);
            }
            return result;
        }

        public DeliveryCompanyModel GetById(int Id)
        {
            string sql = @"SELECT   
        Id ,
        DeliveryCompanyName ,
        DeliveryCompanyCode ,
        IsEnable ,
        SettleType ,
        ClienterFixMoney ,
        ClienterSettleRatio ,
        DeliveryCompanySettleMoney ,
        DeliveryCompanyRatio ,
        BusinessQuantity ,
        ClienterQuantity ,
        CreateTime ,
        CreateName ,
        ModifyName ,
        ModifyTime
   FROM dbo.[DeliveryCompany] WITH(NOLOCK) where Id = @Id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@Id", DbType.Int16).Value = Id;


            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, dbParameters);
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            return MapRows<DeliveryCompanyModel>(dt)[0];
        }

        public DeliveryCompanyModel GetByName(string deliveryCompanyName)
        {
            string sql = @"SELECT   
        Id ,
        DeliveryCompanyName ,
        DeliveryCompanyCode ,
        IsEnable ,
        SettleType ,
        ClienterFixMoney ,
        ClienterSettleRatio ,
        DeliveryCompanySettleMoney ,
        DeliveryCompanyRatio ,
        BusinessQuantity ,
        ClienterQuantity ,
        CreateTime ,
        CreateName ,
        ModifyName ,
        ModifyTime
   FROM dbo.[DeliveryCompany] WITH(NOLOCK) where DeliveryCompanyName = @DeliveryCompanyName";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@DeliveryCompanyName", DbType.String).Value = deliveryCompanyName; 
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, dbParameters);
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            return MapRows<DeliveryCompanyModel>(dt)[0]; 
        }

        /// <summary>
        /// 根据骑士id获取骑士所属物流公司(不管物流公司是否启用)
        /// </summary>
        /// <param name="clienterID"></param>
        /// <returns></returns>
        public DeliveryCompanyModel GetDeliveryCompanyByClienterID(int clienterID)
        {
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string cacheKey = string.Format(RedissCacheKey.ClienterGetDeliveryCompany, clienterID);//缓存的KEY
            DeliveryCompanyModel model = redis.Get<DeliveryCompanyModel>(cacheKey);
            if (model != null)
            {
                return model;
            }
            model = GetDeliveryCompanyByClienterIDFromDB(clienterID);
            if (model==null)
            {
                model = new DeliveryCompanyModel();
            }
            redis.Set(cacheKey, model);
            return model;
        }
        private DeliveryCompanyModel GetDeliveryCompanyByClienterIDFromDB(int clienterID)
        {
            string sql = @"SELECT b.Id ,
                                    b.DeliveryCompanyName ,
                                    b.DeliveryCompanyCode ,
                                    b.IsEnable ,
                                    b.SettleType ,
                                    b.ClienterFixMoney ,
                                    b.ClienterSettleRatio ,
                                    b.DeliveryCompanySettleMoney ,
                                    b.DeliveryCompanyRatio ,
                                    b.BusinessQuantity ,
                                    b.ClienterQuantity ,
                                    b.CreateTime ,
                                    b.CreateName ,
                                    b.ModifyName ,
                                    b.ModifyTime
                            FROM    clienter c ( NOLOCK )
                                    JOIN DeliveryCompany b ( NOLOCK ) ON c.DeliveryCompanyId = b.Id
                            WHERE   c.id = @clienterID";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@clienterID",DbType.Int32).Value= clienterID;
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, dbParameters);
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            return MapRows<DeliveryCompanyModel>(dt)[0];
        }
        /// <summary>
        /// 更新一个物流公司的所有redis缓存
        /// </summary>
        /// <param name="Id"></param>
        private void UpdateRedis(int Id)
        {
            string sql = @"SELECT   c.id AS clienterID ,
                                    b.Id ,
                                    b.DeliveryCompanyName ,
                                    b.DeliveryCompanyCode ,
                                    b.IsEnable ,
                                    b.SettleType ,
                                    b.ClienterFixMoney ,
                                    b.ClienterSettleRatio ,
                                    b.DeliveryCompanySettleMoney ,
                                    b.DeliveryCompanyRatio ,
                                    b.BusinessQuantity ,
                                    b.ClienterQuantity ,
                                    b.CreateTime ,
                                    b.CreateName ,
                                    b.ModifyName ,
                                    b.ModifyTime
                           FROM     clienter c ( NOLOCK )
                                    JOIN DeliveryCompany b ( NOLOCK ) ON c.DeliveryCompanyId = b.Id
                            where b.id=@id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@id", DbType.Int32).Value = Id;
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, dbParameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                string clienterID = "";
                string cacheKey = "";
                DeliveryCompanyModel company = null;
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                foreach (DataRow item in dt.Rows)
                {
                    if (company == null)
                    {
                        company = new DeliveryCompanyModel();
                        company.Id = ParseHelper.ToInt(item["Id"]);
                        company.DeliveryCompanyName = ParseHelper.ToString(item["DeliveryCompanyName"]);
                        company.DeliveryCompanyCode = ParseHelper.ToString(item["DeliveryCompanyCode"]);
                        company.IsEnable = ParseHelper.ToInt(item["IsEnable"]);
                        company.SettleType = ParseHelper.ToInt(item["SettleType"]);
                        company.ClienterFixMoney = ParseHelper.ToDecimal(item["ClienterFixMoney"]);
                        company.ClienterSettleRatio = ParseHelper.ToDecimal(item["ClienterSettleRatio"]);
                        company.DeliveryCompanySettleMoney = ParseHelper.ToDecimal(item["DeliveryCompanySettleMoney"]);
                        company.DeliveryCompanyRatio = ParseHelper.ToDecimal(item["DeliveryCompanyRatio"]);
                        company.BusinessQuantity = ParseHelper.ToInt(item["BusinessQuantity"]);
                        company.ClienterQuantity = ParseHelper.ToInt(item["ClienterQuantity"]);
                        company.CreateTime = ParseHelper.ToDatetime(item["CreateTime"]);
                        company.CreateName = ParseHelper.ToString(item["CreateName"]);
                        company.ModifyName = ParseHelper.ToString(item["ModifyName"]);
                        company.ModifyTime = ParseHelper.ToDatetime(item["ModifyTime"]);
                    }
                    clienterID = ParseHelper.ToString(item["clienterID"]);
                    cacheKey = string.Format(RedissCacheKey.ClienterGetDeliveryCompany, clienterID);//缓存的KEY
                    redis.Set(cacheKey, company);
                }
            }
        }
    }
}
