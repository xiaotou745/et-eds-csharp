use superman
go


--商户金融账号表 新增字段
alter table dbo.BusinessFinanceAccount add IDCard VARCHAR(90) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'个人账户时是身份证号，公司账户时候是营业执照号', 'SCHEMA', N'dbo', 'TABLE', N'BusinessFinanceAccount', 'COLUMN', N'IDCard'
GO

alter table dbo.BusinessFinanceAccount add OpenProvince VARCHAR(45) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'开户省', 'SCHEMA', N'dbo', 'TABLE', N'BusinessFinanceAccount', 'COLUMN', N'OpenProvince'
GO

alter table dbo.BusinessFinanceAccount add OpenCity VARCHAR(45) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'开户省市', 'SCHEMA', N'dbo', 'TABLE', N'BusinessFinanceAccount', 'COLUMN', N'OpenCity'
GO

alter table dbo.BusinessFinanceAccount add YeepayKey VARCHAR(45) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'易宝key', 'SCHEMA', N'dbo', 'TABLE', N'BusinessFinanceAccount', 'COLUMN', N'YeepayKey'
GO
alter table dbo.BusinessFinanceAccount add YeepayStatus SMALLINT default 0 
EXEC sp_addextendedproperty N'MS_Description', N'易宝账户状态  0正常 1失败', 'SCHEMA', N'dbo', 'TABLE', N'BusinessFinanceAccount', 'COLUMN', N'YeepayStatus'
GO



--骑士金融账号表 新增字段
alter table dbo.ClienterFinanceAccount add IDCard VARCHAR(90) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'身份证号', 'SCHEMA', N'dbo', 'TABLE', N'ClienterFinanceAccount', 'COLUMN', N'IDCard'
GO

alter table dbo.ClienterFinanceAccount add OpenProvince VARCHAR(45) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'开户省', 'SCHEMA', N'dbo', 'TABLE', N'ClienterFinanceAccount', 'COLUMN', N'OpenProvince'
GO

alter table dbo.ClienterFinanceAccount add OpenCity VARCHAR(45) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'开户省市', 'SCHEMA', N'dbo', 'TABLE', N'ClienterFinanceAccount', 'COLUMN', N'OpenCity'
GO

alter table dbo.ClienterFinanceAccount add YeepayKey VARCHAR(45) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'易宝key', 'SCHEMA', N'dbo', 'TABLE', N'ClienterFinanceAccount', 'COLUMN', N'YeepayKey'
GO

alter table dbo.ClienterFinanceAccount add YeepayStatus SMALLINT default 0 
EXEC sp_addextendedproperty N'MS_Description', N'易宝账户状态  0正常 1失败', 'SCHEMA', N'dbo', 'TABLE', N'ClienterFinanceAccount', 'COLUMN', N'YeepayStatus'
GO

