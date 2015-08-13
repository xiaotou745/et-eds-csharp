using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Extension;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Util;

namespace Ets.Dao.Finance
{
    /// <summary>
    /// 数据访问类ImprestBalanceRecordDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-08-12 17:03:48
    ///  caoheyang 
    /// </summary>

    public class ImprestBalanceRecordDao : DaoBase
    {
        /// <summary>
        /// 增加一条记录
        /// </summary>
        public int Insert(ImprestBalanceRecord imprestBalanceRecord)
        {
            const string insertSql = @"
insert into ImprestBalanceRecord(Amount,BeforeAmount,AfterAmount,OptName,OptType,Remark,ClienterName,ClienterPhoneNo,ImprestReceiver)
values(@Amount,@BeforeAmount,@AfterAmount,@OptName,@OptType,@Remark,@ClienterName,@ClienterPhoneNo,@ImprestReceiver)

select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Amount", imprestBalanceRecord.Amount);
            dbParameters.AddWithValue("BeforeAmount", imprestBalanceRecord.BeforeAmount);
            dbParameters.AddWithValue("AfterAmount", imprestBalanceRecord.AfterAmount);
            dbParameters.AddWithValue("OptName", imprestBalanceRecord.OptName);
            dbParameters.AddWithValue("OptType", imprestBalanceRecord.OptType);
            dbParameters.AddWithValue("Remark", imprestBalanceRecord.Remark);
            dbParameters.AddWithValue("ClienterName", imprestBalanceRecord.ClienterName);
            dbParameters.AddWithValue("ClienterPhoneNo", imprestBalanceRecord.ClienterPhoneNo);
            dbParameters.AddWithValue("ImprestReceiver", imprestBalanceRecord.ImprestReceiver);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));
        }
        /// <summary>
        /// 更新一条记录
        /// </summary>
        public void Update(ImprestBalanceRecord imprestBalanceRecord)
        {
            const string updateSql = @"
update  ImprestBalanceRecord
set  Amount=@Amount,BeforeAmount=@BeforeAmount,AfterAmount=@AfterAmount,OptName=@OptName,OptTime=@OptTime,OptType=@OptType,Remark=@Remark,ClienterName=@ClienterName,ClienterPhoneNo=@ClienterPhoneNo,
ImprestReceiver=@ImprestReceiver 
where  Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", imprestBalanceRecord.Id);
            dbParameters.AddWithValue("Amount", imprestBalanceRecord.Amount);
            dbParameters.AddWithValue("BeforeAmount", imprestBalanceRecord.BeforeAmount);
            dbParameters.AddWithValue("AfterAmount", imprestBalanceRecord.AfterAmount);
            dbParameters.AddWithValue("OptName", imprestBalanceRecord.OptName);
            dbParameters.AddWithValue("OptTime", imprestBalanceRecord.OptTime);
            dbParameters.AddWithValue("OptType", imprestBalanceRecord.OptType);
            dbParameters.AddWithValue("Remark", imprestBalanceRecord.Remark);
            dbParameters.AddWithValue("ClienterName", imprestBalanceRecord.ClienterName);
            dbParameters.AddWithValue("ClienterPhoneNo", imprestBalanceRecord.ClienterPhoneNo);
            dbParameters.AddWithValue("ImprestReceiver", imprestBalanceRecord.ImprestReceiver);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        public ImprestBalanceRecord GetById(int id)
        {
            const string getbyidSql = @"
select  Id,Amount,BeforeAmount,AfterAmount,OptName,OptTime,OptType,Remark,ClienterName,ClienterPhoneNo,ImprestReceiver
from  ImprestBalanceRecord (nolock)
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, getbyidSql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                return DataTableHelper.ConvertDataTableList<ImprestBalanceRecord>(dt)[0];
            }
            return null;
		}

        /// <summary>
        /// 查询备用金流水列表  add by 彭宜  20150812
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<ImprestBalanceRecordModel> GetImprestBalanceRecordList(ImprestBalanceRecordSearchCriteria criteria)
        {
            string columnList;
            //查询备用金充值
            if (criteria.OptType == 1)
            {
                columnList = @"  Id,
                                    Amount,
                                    OptName,
                                    OptTime,
                                    Remark,
                                    ImprestReceiver";
            }
            //骑士支出
            else
            {
                columnList = @"  Id,
                                    Amount,
                                    OptName,
                                    OptTime,
                                    Remark,
                                    ClienterName,
                                    ClienterPhoneNo";
            }
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            sbSqlWhere.AppendFormat(" AND OptType={0} ", criteria.OptType);
            if (!string.IsNullOrWhiteSpace(criteria.ClienterPhoneNo))
            {
                sbSqlWhere.AppendFormat(" AND ClienterPhoneNo='{0}' ", criteria.ClienterPhoneNo.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.OptDateStart))
            {
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),OptTime,120)>=CONVERT(CHAR(10),'{0}',120) ", criteria.OptDateStart.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.OptDateEnd))
            {
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),OptTime,120)<=CONVERT(CHAR(10),'{0}',120) ", criteria.OptDateEnd.Trim());
            }
            string tableList = @" ImprestBalanceRecord with(nolock)";
            string orderByColumn = " Id ";
            return new PageHelper().GetPages<ImprestBalanceRecordModel>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }


        /// <summary>
        /// 增加一条准备金支出记录  茹化肖
        /// </summary>
        public bool InsertRecord(ImprestBalanceRecord imprestBalanceRecord)
        {
            const string insertSql = @"
  INSERT INTO dbo.ImprestBalanceRecord
          ( Amount ,
            BeforeAmount ,
            AfterAmount ,
            OptName ,
            OptType ,
            Remark ,
            ClienterName ,
            ClienterPhoneNo ,
          )
  VALUES  ( @Amount ,
            @BeforeAmount ,
            @AfterAmount ,
            @OptName ,
            @OptType ,
            @Remark ,
            @ClienterName ,
            @ClienterPhoneNo ,
          )
 
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@Amount", imprestBalanceRecord.Amount);
            dbParameters.AddWithValue("@BeforeAmount", imprestBalanceRecord.BeforeAmount);
            dbParameters.AddWithValue("@AfterAmount", imprestBalanceRecord.AfterAmount);
            dbParameters.AddWithValue("@OptName", imprestBalanceRecord.OptName);
            dbParameters.AddWithValue("@OptType", imprestBalanceRecord.OptType);
            dbParameters.AddWithValue("@Remark", imprestBalanceRecord.Remark);
            dbParameters.AddWithValue("@ClienterName", imprestBalanceRecord.ClienterName);
            dbParameters.AddWithValue("@ClienterPhoneNo", imprestBalanceRecord.ClienterPhoneNo);
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, insertSql, dbParameters))>0;
        }


        /// <summary>
        /// 备用金充值功能 add by caoheyang 20150813
        /// </summary>
        /// <param name="imprestBalanceRecord"></param>
        /// <returns></returns>
        public bool InsertRechargeRecord(ImprestBalanceRecord imprestBalanceRecord)
        {
            const string insertSql = @"
update dbo.ImprestRecharge set TotalRecharge=TotalRecharge+@Amount,RemainingAmount=RemainingAmount+@Amount
output @Amount,inserted.RemainingAmount-@Amount,Inserted.RemainingAmount, @OptName,@OptType,@Remark,@ImprestReceiver
into ImprestBalanceRecord(Amount,BeforeAmount,AfterAmount,OptName,OptType,Remark,ImprestReceiver)
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@Amount", imprestBalanceRecord.Amount);
            dbParameters.AddWithValue("@OptName", imprestBalanceRecord.OptName);
            dbParameters.AddWithValue("@OptType", imprestBalanceRecord.OptType);
            dbParameters.AddWithValue("@Remark", imprestBalanceRecord.Remark);
            dbParameters.AddWithValue("@ImprestReceiver", imprestBalanceRecord.ImprestReceiver);;
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, insertSql, dbParameters)) > 0;
        }
    }
}
