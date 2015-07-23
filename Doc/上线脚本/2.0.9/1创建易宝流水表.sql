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
EXEC sp_addextendedproperty N'MS_Description', N'�ױ�֧����¼��', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'����id', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Id'
GO
EXEC sp_addextendedproperty N'MS_Description', N'����� �����ʺ���Ψһ MAX(50 )', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'RequestId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�̻���� �״��͹�˾���˺� ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'CustomerNumber'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�̻���Կ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'HmacKey'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�ױ����˻�����    ledgerno�ǿ�sourceledgernoΪ��ʱ�����˻�ת���˻���customernumber �� ledgerno��', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Ledgerno'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�ױ����˻�����    ledgernoΪ��sourceledgerno�ǿ�ʱ�����˻�ת���˻���sourceledgerno �� customernumber��', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'SourceLedgerno'
GO
EXEC sp_addextendedproperty N'MS_Description', N'���', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Amount'
GO
EXEC sp_addextendedproperty N'MS_Description', N'0  ת�� 1�������� 2�ص�����', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'TransferType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'֧���� 0 ���˻� 1 ���˻�', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Payer'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�ױ�����״̬��', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Code'
GO
EXEC sp_addextendedproperty N'MS_Description', N'ǩ����Ϣ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Hmac'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�ױ�������Ϣ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Msg'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�����ױ��ӿڻص���ַ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'CallbackUrl'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�ױ��ص�����״̬', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Status'
GO
EXEC sp_addextendedproperty N'MS_Description', N'���ֵ���', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'WithdrawId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�û����ͣ�0��ʿ 1�̼�  Ĭ�� 0��', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'UserType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'���ź���λ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Lastno'
GO
EXEC sp_addextendedproperty N'MS_Description', N'����״̬����', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Desc'
GO
EXEC sp_addextendedproperty N'MS_Description', N'ʱ��', 'SCHEMA', N'dbo', 'TABLE', N'YeePayRecord', 'COLUMN', N'Addtime'
GO