use  superman
go
--drop table [ClienterPushLog]

CREATE TABLE [dbo].[ClienterPushLog]
(
[ID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderId] [bigint] NOT NULL,
[ClienterIds] VARCHAR(2048) NOT NULL,
[CreateTime] [datetime] NOT NULL CONSTRAINT [DF__ClienterPushLog__CreateTime] DEFAULT (getdate()),
ProcessTime [datetime]  NULL 
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ClienterPushLog] ADD CONSTRAINT [PK_ClienterPushLog] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'闪送骑士推单记录表', 'SCHEMA', N'dbo', 'TABLE', N'ClienterPushLog', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'主键 自增ID', 'SCHEMA', N'dbo', 'TABLE', N'ClienterPushLog', 'COLUMN', N'ID'
GO
EXEC sp_addextendedproperty N'MS_Description', N'订单id', 'SCHEMA', N'dbo', 'TABLE', N'ClienterPushLog', 'COLUMN', N'OrderId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'推送骑士id集合 以;id;id;格式存储', 'SCHEMA', N'dbo', 'TABLE', N'ClienterPushLog', 'COLUMN', N'ClienterIds'
GO
EXEC sp_addextendedproperty N'MS_Description', N'新订单推送时间', 'SCHEMA', N'dbo', 'TABLE', N'ClienterPushLog', 'COLUMN', N'CreateTime'
GO
EXEC sp_addextendedproperty N'MS_Description', N'订单处理后（接单取消订单）推送时间', 'SCHEMA', N'dbo', 'TABLE', N'ClienterPushLog', 'COLUMN', N'ProcessTime'
GO
