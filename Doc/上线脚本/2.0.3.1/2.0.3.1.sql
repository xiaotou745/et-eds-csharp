alter table dbo.[group] add IsModifyBind int default 0 not null;
go

alter   table     business   drop   constraint   [DF_business_Longitude]
alter   table     business   drop   constraint   [DF_business_Latitude]
alter   table     business   drop   constraint   [DF_business_Status]
alter   table     business   drop   constraint   [DF_business_InsertTime]
alter   table     business   drop   constraint   [DF_business_districtId]
alter   table     business   drop   constraint   [DF_business_GroupId]
alter   table     business   drop   constraint   [DF_business_OriginalBusiId]
go
 ALTER   TABLE   business   ADD    CONSTRAINT  [DF_business_Longitude]   DEFAULT   0   FOR   Longitude
 ALTER   TABLE   business   ADD    CONSTRAINT  [DF_business_Latitude]   DEFAULT   0   FOR   Latitude
 ALTER   TABLE   business   ADD    CONSTRAINT  [DF_business_Status]   DEFAULT   0   FOR   Status
 ALTER   TABLE   business   ADD    CONSTRAINT  [DF_business_InsertTime]   DEFAULT   GETDATE()   FOR   InsertTime
 ALTER   TABLE   business   ADD    CONSTRAINT  [DF_business_districtId]   DEFAULT   0   FOR   districtId
 ALTER   TABLE   business   ADD    CONSTRAINT  [DF_business_GroupId]   DEFAULT   0   FOR   districtId
 ALTER   TABLE   business   ADD    CONSTRAINT  [DF_business_OriginalBusiId]   DEFAULT   0   FOR   districtId
go



update business set PhoneNo2=PhoneNo
where Id in(
select id from business where isnull(PhoneNo2,'')=''
)


----------------更新商家所属集团是否允许修改
update dbo.[group] set IsModifyBind=0
update dbo.[group] set IsModifyBind=1 where id =4