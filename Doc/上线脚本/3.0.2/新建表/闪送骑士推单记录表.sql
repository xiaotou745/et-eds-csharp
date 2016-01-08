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
EXEC sp_addextendedproperty N'MS_Description', N'������ʿ�Ƶ���¼��', 'SCHEMA', N'dbo', 'TABLE', N'ClienterPushLog', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'���� ����ID', 'SCHEMA', N'dbo', 'TABLE', N'ClienterPushLog', 'COLUMN', N'ID'
GO
EXEC sp_addextendedproperty N'MS_Description', N'����id', 'SCHEMA', N'dbo', 'TABLE', N'ClienterPushLog', 'COLUMN', N'OrderId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'������ʿid���� ��;id;id;��ʽ�洢', 'SCHEMA', N'dbo', 'TABLE', N'ClienterPushLog', 'COLUMN', N'ClienterIds'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�¶�������ʱ��', 'SCHEMA', N'dbo', 'TABLE', N'ClienterPushLog', 'COLUMN', N'CreateTime'
GO
EXEC sp_addextendedproperty N'MS_Description', N'��������󣨽ӵ�ȡ������������ʱ��', 'SCHEMA', N'dbo', 'TABLE', N'ClienterPushLog', 'COLUMN', N'ProcessTime'
GO
