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
                dbParameters.Add("@ClienterSettleRatio", DbType.Decimal).Value = deliveryCompanyModel.ClienterSettleRatio;
                dbParameters.Add("@DeliveryCompanyRatio", DbType.Decimal).Value = deliveryCompanyModel.DeliveryCompanyRatio;

                dbParameters.Add("@ClienterFixMoney", DbType.Decimal).Value = 0;
                dbParameters.Add("@DeliveryCompanySettleMoney", DbType.Decimal).Value = 0;
            }
            else
            {
                dbParameters.Add("@ClienterFixMoney", DbType.Decimal).Value = deliveryCompanyModel.ClienterFixMoney;
                dbParameters.Add("@DeliveryCompanySettleMoney", DbType.Decimal).Value = deliveryCompanyModel.DeliveryCompanySettleMoney;

                dbParameters.Add("@ClienterSettleRatio", DbType.Decimal).Value = 0;
                dbParameters.Add("@DeliveryCompanyRatio", DbType.Decimal).Value = 0;
            }
            dbParameters.Add("@CreateName", DbType.String).Value = deliveryCompanyModel.CreateName;
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));
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
            upSql.Append(@"where   Id = @Id;");
            IDbParameters dbParameters = DbHelper.CreateDbParameters();

            dbParameters.Add("@Id", DbType.Int32).Value = deliveryCompanyModel.Id;
            dbParameters.Add("@DeliveryCompanyName", DbType.String).Value = deliveryCompanyModel.DeliveryCompanyName;
            dbParameters.Add("@SettleType", DbType.Int16).Value = deliveryCompanyModel.SettleType;
            dbParameters.Add("@IsDisplay", DbType.Int16).Value = deliveryCompanyModel.IsDisplay;
            dbParameters.Add("@IsEnable", DbType.Int16).Value = deliveryCompanyModel.IsEnable;
            if (deliveryCompanyModel.SettleType == 1)
            {
                dbParameters.Add("@ClienterSettleRatio", DbType.Decimal).Value = deliveryCompanyModel.ClienterSettleRatio;
                dbParameters.Add("@DeliveryCompanyRatio", DbType.Decimal).Value = deliveryCompanyModel.DeliveryCompanyRatio;

                dbParameters.Add("@ClienterFixMoney", DbType.Decimal).Value = 0;
                dbParameters.Add("@DeliveryCompanySettleMoney", DbType.Decimal).Value = 0;
            }
            else
            {
                dbParameters.Add("@ClienterFixMoney", DbType.Decimal).Value = deliveryCompanyModel.ClienterFixMoney;
                dbParameters.Add("@DeliveryCompanySettleMoney", DbType.Decimal).Value = deliveryCompanyModel.DeliveryCompanySettleMoney;

                dbParameters.Add("@ClienterSettleRatio", DbType.Decimal).Value = 0;
                dbParameters.Add("@DeliveryCompanyRatio", DbType.Decimal).Value = 0;
            }
            dbParameters.Add("@ModifyName", DbType.String).Value = deliveryCompanyModel.ModifyName;
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, upSql.ToString(), dbParameters));
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
    }
}
