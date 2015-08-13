using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.DataModel.Finance;
using ETS.Util;

namespace Ets.Dao.Finance
{
    /// <summary>
    /// 数据访问类ImprestRechargeDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-08-12 16:49:48
    /// add by caoheyang  20150812
    /// </summary>
    public class ImprestRechargeDao : DaoBase
    {
        #region IImprestRechargeRepos  Members

		/// <summary>
		/// 增加一条记录
		/// </summary>
		public int Insert(ImprestRecharge imprestRecharge)
		{
			const string insertSql = @"
insert into ImprestRecharge(TotalRecharge,RemainingAmount,TotalPayment)
values(@TotalRecharge,@RemainingAmount,@TotalPayment)

select @@IDENTITY";
			IDbParameters dbParameters = DbHelper.CreateDbParameters();
			dbParameters.AddWithValue("TotalRecharge", imprestRecharge.TotalRecharge);
			dbParameters.AddWithValue("RemainingAmount", imprestRecharge.RemainingAmount);
			dbParameters.AddWithValue("TotalPayment", imprestRecharge.TotalPayment);
		    return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));		
		}

		/// <summary>
		/// 更新一条记录
		/// </summary>
		public void Update(ImprestRecharge imprestRecharge)
		{
			const string updateSql = @"
update  ImprestRecharge
set  TotalRecharge=@TotalRecharge,RemainingAmount=@RemainingAmount,TotalPayment=@TotalPayment,CreateTime=@CreateTime
where  Id=@Id ";
			IDbParameters dbParameters = DbHelper.CreateDbParameters();
			dbParameters.AddWithValue("Id", imprestRecharge.Id);
			dbParameters.AddWithValue("TotalRecharge", imprestRecharge.TotalRecharge);
			dbParameters.AddWithValue("RemainingAmount", imprestRecharge.RemainingAmount);
			dbParameters.AddWithValue("TotalPayment", imprestRecharge.TotalPayment);
			dbParameters.AddWithValue("CreateTime", imprestRecharge.CreateTime);
			DbHelper.ExecuteNonQuery(SuperMan_Write,  updateSql, dbParameters);
		}



		/// <summary>
		/// 根据ID获取对象
		/// </summary>
		public ImprestRecharge GetById(int id)
		{

			const string getbyidSql = @"
select  Id,TotalRecharge,RemainingAmount,TotalPayment,CreateTime
from  ImprestRecharge (nolock)
where  Id=@Id ";
     			IDbParameters dbParameters = DbHelper.CreateDbParameters();
          	dbParameters.AddWithValue("Id", id);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, getbyidSql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                return DataTableHelper.ConvertDataTableList<ImprestRecharge>(dt)[0];
            }
            return null;
		}

		#endregion

        /// <summary>
        /// 获取备用金余额(不锁库)
        /// 2015年8月12日17:51:49
        /// 茹化肖
        /// </summary>
        public int GetRemainingAmountNoLock()
        {
            const string getbyidSql = @"SELECT ISNULL(RemainingAmount,0) AS RemainingAmount FROM ImprestRecharge (NOLOCK) ";
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, getbyidSql), 0);
        }

        /// <summary>
        /// 获取备用金信息(锁库)
        /// 2015年8月12日17:51:49
        /// 茹化肖
        /// </summary>
        public ImprestRecharge GetRemainingAmountLock()
        {
            const string getbyidSql = @"
select  Id,TotalRecharge,RemainingAmount,TotalPayment,CreateTime
from  ImprestRecharge";
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, getbyidSql));
            if (DataTableHelper.CheckDt(dt))
            {
                return DataTableHelper.ConvertDataTableList<ImprestRecharge>(dt)[0];
            }
            return null;
        }

        /// <summary>
        /// 备用金支出
        /// 2015年8月12日19:53:10
        /// 茹化肖
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool ImprestRechargePayOut(decimal price,int ID)
        {
            const string updateSql = @"
UPDATE  ImprestRecharge SET RemainingAmount=(RemainingAmount-@Price),TotalPayment=(TotalPayment+@Price) 
WHERE Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@Price", DbType.Decimal).Value=price;
            dbParameters.Add("@Id", DbType.Int32).Value=ID;
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql,dbParameters), 0)>0;
        }

        //UPDATE  ImprestRecharge SET RemainingAmount=(RemainingAmount-@Price),TotalPayment=(TotalPayment+@Price) WHERE Id=1
	}
}
