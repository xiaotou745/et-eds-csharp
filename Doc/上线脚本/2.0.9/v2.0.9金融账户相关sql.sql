use superman
go


--�̻������˺ű� �����ֶ�
alter table dbo.BusinessFinanceAccount add IDCard VARCHAR(90) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'�����˻�ʱ�����֤�ţ���˾�˻�ʱ����Ӫҵִ�պ�', 'SCHEMA', N'dbo', 'TABLE', N'BusinessFinanceAccount', 'COLUMN', N'IDCard'
GO

alter table dbo.BusinessFinanceAccount add OpenProvince VARCHAR(45) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'����ʡ', 'SCHEMA', N'dbo', 'TABLE', N'BusinessFinanceAccount', 'COLUMN', N'OpenProvince'
GO

alter table dbo.BusinessFinanceAccount add OpenCity VARCHAR(45) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'����ʡ��', 'SCHEMA', N'dbo', 'TABLE', N'BusinessFinanceAccount', 'COLUMN', N'OpenCity'
GO

alter table dbo.BusinessFinanceAccount add YeepayKey VARCHAR(45) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'�ױ�key', 'SCHEMA', N'dbo', 'TABLE', N'BusinessFinanceAccount', 'COLUMN', N'YeepayKey'
GO
alter table dbo.BusinessFinanceAccount add YeepayStatus SMALLINT default 0 
EXEC sp_addextendedproperty N'MS_Description', N'�ױ��˻�״̬  0���� 1ʧ��', 'SCHEMA', N'dbo', 'TABLE', N'BusinessFinanceAccount', 'COLUMN', N'YeepayStatus'
GO



--��ʿ�����˺ű� �����ֶ�
alter table dbo.ClienterFinanceAccount add IDCard VARCHAR(90) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'���֤��', 'SCHEMA', N'dbo', 'TABLE', N'ClienterFinanceAccount', 'COLUMN', N'IDCard'
GO

alter table dbo.ClienterFinanceAccount add OpenProvince VARCHAR(45) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'����ʡ', 'SCHEMA', N'dbo', 'TABLE', N'ClienterFinanceAccount', 'COLUMN', N'OpenProvince'
GO

alter table dbo.ClienterFinanceAccount add OpenCity VARCHAR(45) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'����ʡ��', 'SCHEMA', N'dbo', 'TABLE', N'ClienterFinanceAccount', 'COLUMN', N'OpenCity'
GO

alter table dbo.ClienterFinanceAccount add YeepayKey VARCHAR(45) default '' 
EXEC sp_addextendedproperty N'MS_Description', N'�ױ�key', 'SCHEMA', N'dbo', 'TABLE', N'ClienterFinanceAccount', 'COLUMN', N'YeepayKey'
GO

alter table dbo.ClienterFinanceAccount add YeepayStatus SMALLINT default 0 
EXEC sp_addextendedproperty N'MS_Description', N'�ױ��˻�״̬  0���� 1ʧ��', 'SCHEMA', N'dbo', 'TABLE', N'ClienterFinanceAccount', 'COLUMN', N'YeepayStatus'
GO

