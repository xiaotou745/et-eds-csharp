use superman
go
drop table [OpenCallBackLog]
go
CREATE TABLE [dbo].[OpenCallBackLog]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Url] [nvarchar] (800) COLLATE Chinese_PRC_CI_AS NOT NULL CONSTRAINT [DF_OpenCallBackLog_Url] DEFAULT (''),
[OrderId] [bigint] NOT NULL CONSTRAINT [DF_OpenCallBackLog_OrderId] DEFAULT ((0)),
[OrderNo] [nvarchar] (90)  NOT NULL CONSTRAINT [DF_OpenCallBackLog_OrderNo] DEFAULT (''),
[RequestBody] [nvarchar] (800) COLLATE Chinese_PRC_CI_AS NOT NULL CONSTRAINT [DF_OpenCallBackLog_RequestBody] DEFAULT (''),
[ResponseBody] [nvarchar] (800) COLLATE Chinese_PRC_CI_AS NOT NULL CONSTRAINT [DF_OpenCallBackLog_ResponseBody] DEFAULT (''),
[Status] [int] NOT NULL CONSTRAINT [DF_OpenCallBackLog_Status] DEFAULT ((0)),
[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_OpenCallBackLog_CreateTime] DEFAULT (getdate())
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OpenCallBackLog] ADD CONSTRAINT [PK_OpenCallBackLog] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'请求地址URL', 'SCHEMA', N'dbo', 'TABLE', N'OpenCallBackLog', 'COLUMN', N'Url'
GO
EXEC sp_addextendedproperty N'MS_Description', N'订单id', 'SCHEMA', N'dbo', 'TABLE', N'OpenCallBackLog', 'COLUMN', N'OrderId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'订单号', 'SCHEMA', N'dbo', 'TABLE', N'OpenCallBackLog', 'COLUMN', N'OrderNo'
GO
EXEC sp_addextendedproperty N'MS_Description', N'请求体(包括get post参数)', 'SCHEMA', N'dbo', 'TABLE', N'OpenCallBackLog', 'COLUMN', N'RequestBody'
GO
EXEC sp_addextendedproperty N'MS_Description', N'响应体(JSON)', 'SCHEMA', N'dbo', 'TABLE', N'OpenCallBackLog', 'COLUMN', N'ResponseBody'
GO
EXEC sp_addextendedproperty N'MS_Description', N'订单状态', 'SCHEMA', N'dbo', 'TABLE', N'OpenCallBackLog', 'COLUMN', N'Status'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建时间', 'SCHEMA', N'dbo', 'TABLE', N'OpenCallBackLog', 'COLUMN', N'CreateTime'
GO
