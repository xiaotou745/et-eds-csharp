use superman
go

--drop table  [YeePayRecord]
CREATE TABLE [dbo].[YeePayRecord]
(
[Id] [bigint] NOT NULL IDENTITY(1, 1),
[RequestId] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[CustomerNumber] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[HmacKey] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL,
[Ledgerno] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL,
[SourceLedgerno] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL,
[Amount] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[TransferType] [smallint] NULL,
[Payer] [smallint] NOT NULL,
[Code] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL,
[Hmac] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL,
[Msg] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL,
[CallbackUrl] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL,
[Status] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL,
[WithdrawId] [bigint] NOT NULL,
[UserType] [smallint] NOT NULL,
[Lastno] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS NULL,
[Desc] [nvarchar] (256) COLLATE Chinese_PRC_CI_AS null,
[Addtime] [datetime] NOT NULL CONSTRAINT [DF__YeePayRecord__Addti__4E5E8EA2] DEFAULT (getdate())
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[YeePayRecord] ADD CONSTRAINT [PK_YEEPAYRECORD] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'易宝支付记录表', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'主键id', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Id'
GO
EXEC sp_addextendedproperty N'MS_Description', N'请求号 在主帐号下唯一 MAX(50 )', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'RequestId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'商户编号 易代送公司主账号 ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'CustomerNumber'
GO
EXEC sp_addextendedproperty N'MS_Description', N'商户密钥', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'HmacKey'
GO
EXEC sp_addextendedproperty N'MS_Description', N'易宝子账户编码    ledgerno非空sourceledgerno为空时：主账户转子账户（customernumber → ledgerno）', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Ledgerno'
GO
EXEC sp_addextendedproperty N'MS_Description', N'易宝子账户编码    ledgerno为空sourceledgerno非空时：子账户转主账户（sourceledgerno → customernumber）', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'SourceLedgerno'
GO
EXEC sp_addextendedproperty N'MS_Description', N'金额', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Amount'
GO
EXEC sp_addextendedproperty N'MS_Description', N'0  转账 1发起提现 2回调提现', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'TransferType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'支出方 0 主账户 1 子账户', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Payer'
GO
EXEC sp_addextendedproperty N'MS_Description', N'易宝返回状态吗', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Code'
GO
EXEC sp_addextendedproperty N'MS_Description', N'签名信息', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Hmac'
GO
EXEC sp_addextendedproperty N'MS_Description', N'易宝返回消息', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Msg'
GO
EXEC sp_addextendedproperty N'MS_Description', N'请求易宝接口回调地址', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'CallbackUrl'
GO
EXEC sp_addextendedproperty N'MS_Description', N'易宝回调返回状态', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Status'
GO
EXEC sp_addextendedproperty N'MS_Description', N'提现单号', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'WithdrawId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'用户类型（0骑士 1商家  默认 0）', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'UserType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'卡号后四位', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Lastno'
GO
EXEC sp_addextendedproperty N'MS_Description', N'提现状态描述', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Desc'
GO
EXEC sp_addextendedproperty N'MS_Description', N'时间', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Addtime'
GO