use superman
go
--新增列
ALTER TABLE dbo.ReceviceAddress ADD UserName NVARCHAR(256) DEFAULT '' NOT NULL
GO 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceviceAddress', @level2type=N'COLUMN',@level2name=N'UserName'
go

UPDATE ra
SET ra.UserName=tbl.UserName
FROM ReceviceAddress ra
JOIN (SELECT    RecevicePhoneNo ,
	ReceviceAddress ,
	businessId ,
	MAX(o.PubDate) AS PubDate,
	ReceviceName UserName
FROM      dbo.[order] o ( NOLOCK )
WHERE     ISNULL(RecevicePhoneNo, '') != '' AND  ISNULL(ReceviceName, '') != ''
GROUP BY  businessId ,
	RecevicePhoneNo ,
	ReceviceAddress,
	ReceviceName) tbl ON tbl.businessId = ra.BusinessId  
						AND tbl.RecevicePhoneNo = ra.PhoneNo                           
						AND ISNULL(tbl.ReceviceAddress,'') = ra.[ADDRESS]
						AND tbl.PubDate = ra.PubDate;