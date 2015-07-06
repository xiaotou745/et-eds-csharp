DECLARE @BeginDate datetime ='2015-01-01'
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
DROP TABLE #TempAdress