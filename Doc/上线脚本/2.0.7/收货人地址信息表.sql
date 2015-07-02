use superman
go
if exists (select 1
            from  sysobjects
           where  id = object_id('ReceviceAddress')
            and   type = 'U')
   drop table ReceviceAddress
go
CREATE TABLE [dbo].[ReceviceAddress]
(
[Id] [bigint] NOT NULL IDENTITY(1, 1),
[PhoneNo] [nvarchar] (90) COLLATE Chinese_PRC_CI_AS NOT NULL,
[Address] [nvarchar] (510) COLLATE Chinese_PRC_CI_AS NOT NULL,
[PubDate] [datetime] NOT NULL,
[AddTime] [datetime] NOT NULL CONSTRAINT [DF__ReceviceA__AddTi__25918339] DEFAULT (getdate()),
[BusinessId] [int] NOT NULL
) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'�ջ��˵�ַ��Ϣ��', 'SCHEMA', N'dbo', 'TABLE', N'ReceviceAddress', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'����ID(PK)', 'SCHEMA', N'dbo', 'TABLE', N'ReceviceAddress', 'COLUMN', N'Id'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�绰����', 'SCHEMA', N'dbo', 'TABLE', N'ReceviceAddress', 'COLUMN', N'PhoneNo'
GO
EXEC sp_addextendedproperty N'MS_Description', N'��ַ', 'SCHEMA', N'dbo', 'TABLE', N'ReceviceAddress', 'COLUMN', N'Address'
GO
EXEC sp_addextendedproperty N'MS_Description', N'������������', 'SCHEMA', N'dbo', 'TABLE', N'ReceviceAddress', 'COLUMN', N'PubDate'
GO
EXEC sp_addextendedproperty N'MS_Description', N'����ʱ��', 'SCHEMA', N'dbo', 'TABLE', N'ReceviceAddress', 'COLUMN', N'AddTime'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�̻�id', 'SCHEMA', N'dbo', 'TABLE', N'ReceviceAddress', 'COLUMN', N'BusinessId'
GO

--��������
create nonclustered index IX_ReceviceAddress_BusinessId_inc on dbo.ReceviceAddress(BusinessId) include(Id,PhoneNo,
address,
PubDate)with(online=on,fillfactor=90) on [PRIMARY]
GO