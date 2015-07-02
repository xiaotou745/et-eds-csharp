using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Order;
using Ets.Model.ParameterModel.Order;

namespace Ets.Dao.Order
{
    /// <summary>
    /// 收货人地址表  add By  caoheyang   20150702
    /// </summary>
    public class ReceviceAddressDao : DaoBase
    {

        /// <summary>
        ///  B端商户拉取收货人地址缓存到本地 add By  caoheyang   20150702 
        /// </summary>
        public IList<ConsigneeAddressBDM> ConsigneeAddressB(ConsigneeAddressBPM model)
        {
            IList<ConsigneeAddressBDM> models = new List<ConsigneeAddressBDM>();
            const string querysql = @"
select  Id ,PhoneNo,Address,PubDate
from    dbo.ReceviceAddress
where   Id > @AddressId
        and BusinessId = @BusinessId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", model.BusinessId);
            dbParameters.AddWithValue("AddressId", model.AddressId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<ConsigneeAddressBDM>(dt);
            }
            return models;
        }

        /// <summary>
        /// 扫描地址数据
        /// </summary>
        /// <param name="beginDate">上次扫描结束时间</param>
        /// <param name="lastId">上次扫描的最后ID</param>
        /// <returns></returns>
        public int GetAddress(DateTime beginDate)
        {
            #region 查询SQL
            const string querysql = @"
SELECT  ODataNew.PhoneNo ,
        ODataNew.Address ,
        ODataNew.businessId ,
        ODataNew.PubDate ,
        CASE WHEN ODataNew.PubDate != RA2.PubDate THEN 1 ELSE 0 END AS Falg
INTO    #TempAdress
FROM    ( SELECT    OData.RecevicePhoneNo AS PhoneNo ,
                    OData.ReceviceAddress AS [Address] ,
                    OData.BusinessId ,
                    OData.PubDate
          FROM      ( SELECT    RecevicePhoneNo ,
                                ReceviceAddress ,
                                businessId ,
                                MAX(o.PubDate) AS PubDate
                      FROM      dbo.[order] o ( NOLOCK )
                      WHERE     o.PubDate > @BeginDate
                                AND ISNULL(RecevicePhoneNo, '') != ''
                      GROUP BY  businessId ,
                                RecevicePhoneNo ,
                                ReceviceAddress
                    ) AS OData
                    LEFT JOIN dbo.ReceviceAddress RA ( NOLOCK ) ON OData.businessId = RA.BusinessId
                                                              AND OData.RecevicePhoneNo = RA.PhoneNo
                                                              AND OData.ReceviceAddress = RA.[ADDRESS]
                                                              AND OData.PubDate = RA.PubDate
          WHERE     RA.id IS NULL
        ) AS ODataNew
        LEFT JOIN dbo.ReceviceAddress RA2 ( NOLOCK ) ON ODataNew.businessId = RA2.BusinessId
                                                        AND ODataNew.PhoneNo = RA2.PhoneNo
                                                        AND ODataNew.[ADDRESS] = RA2.[ADDRESS]
                                                       
INSERT  INTO dbo.ReceviceAddress
        ( PhoneNo ,
          [Address] ,
          Businessid ,
          PubDate
        )
        SELECT  PhoneNo ,
                [Address] ,
                Businessid ,
                PubDate
        FROM    #TempAdress
        WHERE   #TempAdress.Falg = 0
UPDATE  dbo.ReceviceAddress
SET     PubDate = #TempAdress.PubDate
FROM    dbo.ReceviceAddress RA3 ,
        #TempAdress
WHERE   #TempAdress.businessId = RA3.BusinessId
        AND #TempAdress.PhoneNo = RA3.PhoneNo
        AND #TempAdress.[ADDRESS] = RA3.[ADDRESS]                                                   
DROP TABLE #TempAdress";
            #endregion
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BeginDate", beginDate);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, querysql, dbParameters);
        }
    }
}
