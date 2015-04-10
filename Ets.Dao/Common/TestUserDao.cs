using System.Collections.Generic;
using System.Data;
using ETS;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.Common;

namespace Ets.Dao.Common
{
    public class TestUserDao : DaoBase
    {
        /// <summary>
        /// 获取测试账号
        /// </summary>
        /// <returns></returns>
        public IList<TestUserModel> GetTestUserList()
        {
            string sql = @"SELECT T.Id,T.PhoneNo,CASE ISNULL(C.Id,0) WHEN 0 THEN 0 ELSE 1 END AS IsC
                        ,CASE ISNULL(B.Id,0) WHEN 0 THEN 0 ELSE 1 END AS IsB
                          FROM  dbo.TestUserTbl(nolock) T left JOIN dbo.clienter(nolock) C ON T.PhoneNo=C.PhoneNo 
                         left JOIN dbo.business(nolock) B ON T.PhoneNo=B.PhoneNo ";
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql));
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            return ConvertDataTableList<TestUserModel>(dt);
        }

        /// <summary>
        /// 添加测试账号
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool AddTestUser(string phoneNo)
        {
            string sql = @"INSERT INTO TestUserTbl (PhoneNo) VALUES (@PhoneNo);select @@IDENTITY ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("PhoneNo", phoneNo); 
            object i = DbHelper.ExecuteScalar(Config.SuperMan_Write, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString())>0;
            }
            return false;
        }

        /// <summary>
        /// 删除测试账号
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool DelTestUser(string phoneNo)
        {
            string sql = @" delete from TestUserTbl where PhoneNo=@PhoneNo ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("PhoneNo", phoneNo);
            int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql, dbParameters);
            return i > 0;
        }


        /// <summary>
        /// 删除测试骑士
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool DeleteTestClienter(string phoneNo)
        {
            string sql = @" IF EXISTS (SELECT 1 FROM dbo.TestUserTbl(NOLOCK) WHERE PhoneNo=@PhoneNo)
                            BEGIN
                                update dbo.[order] set Status=3 ,clienterId=null where [order].clienterId =
                                (select id from dbo.clienter(nolock) where PhoneNo=@PhoneNo); --更新订单状态为已取消
                                delete from dbo.clienter where PhoneNo=@PhoneNo
                            END";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("PhoneNo", phoneNo);
            int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql, dbParameters);
            return i > 0;
        }

        /// <summary>
        /// 删除测试订单
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool DeleteTestOrder(string phoneNo)
        {
            string sql = @" IF EXISTS (SELECT 1 FROM dbo.TestUserTbl(NOLOCK) WHERE PhoneNo=@PhoneNo)
                            BEGIN
                               delete from [order] where businessId in (SELECT id FROM  dbo.business(nolock) where PhoneNo=@PhoneNo)  --删除订单
                            END ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("PhoneNo", phoneNo);
            int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql, dbParameters);
            return i > 0;
        }

        /// <summary>
        /// 删除测试商家
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool DeleteTestBusiness(string phoneNo)
        {
            string sql = @" IF EXISTS (SELECT 1 FROM dbo.TestUserTbl(NOLOCK) WHERE PhoneNo=@PhoneNo)
                            BEGIN
                                delete from [order] where businessId in (SELECT id FROM  dbo.business(nolock) where PhoneNo=@PhoneNo)  --删除订单
                                delete from business where PhoneNo=@PhoneNo  --删除商家
                            END ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("PhoneNo", phoneNo);
            int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql, dbParameters);
            return i > 0;
        }
    }
}
