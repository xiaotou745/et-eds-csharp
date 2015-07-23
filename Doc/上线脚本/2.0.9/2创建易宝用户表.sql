use superman
go
CREATE TABLE [dbo].[YeePayUser]
(
[Id] [bigint] NOT NULL IDENTITY(1, 1),
[UserId] [int] NOT NULL,
[UserType] [smallint] NOT NULL,
[RequestId] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[CustomerNumberr] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[HmacKey] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[BindMobile] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[CustomerType] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[SignedName] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[LinkMan] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[IdCard] [varchar] (256) COLLATE Chinese_PRC_CI_AS NULL,
[BusinessLicence] [varchar] (256) COLLATE Chinese_PRC_CI_AS NULL,
[LegalPerson] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[MinsettleAmount] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[Riskreserveday] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[BankAccountNumber] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[BankName] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[AccountName] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[BankAccountType] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[BankProvince] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[BankCity] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[ManualSettle] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[Hmac] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[Addtime] [datetime] NOT NULL CONSTRAINT [DF__YeePayUse__Addti__4E5E8EA2] DEFAULT (getdate()),
[Ledgerno] [varchar] (256) COLLATE Chinese_PRC_CI_AS NOT NULL,
[BalanceRecord] [decimal] (18, 0) NOT NULL CONSTRAINT [DF__YeePayUse__Balan__4F52B2DB] DEFAULT ((0))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[YeePayUser] ADD CONSTRAINT [PK_YEEPAYUSER] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'�ױ����˻���
   ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Id������', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'Id'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�û�id ����ʿid/�̻�id��', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'UserId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�û����ͣ�0��ʿ 1�̼�  Ĭ�� 0��', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'UserType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'ע���ױ��˻�ʱ������id  ע������� �����ʺ���Ψһ MAX(50 )', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'RequestId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�̻���� �״��͹�˾���˺� ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'CustomerNumberr'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�̻���Կ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'HmacKey'
GO
EXEC sp_addextendedproperty N'MS_Description', N'���ֻ�', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BindMobile'
GO
EXEC sp_addextendedproperty N'MS_Description', N'ע������  PERSON ������ ENTERPRISE����ҵ ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'CustomerType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'ǩԼ��   �̻�ǩԼ�������ˣ���д��������ҵ����д��ҵ���ơ�', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'SignedName'
GO
EXEC sp_addextendedproperty N'MS_Description', N'��ϵ��', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'LinkMan'
GO
EXEC sp_addextendedproperty N'MS_Description', N'���֤  customertypeΪPERSONʱ������', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'IdCard'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Ӫҵִ�պ� customertypeΪENTERPRISEʱ������', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BusinessLicence'
GO
EXEC sp_addextendedproperty N'MS_Description', N'����  PERSONʱ��idcard��Ӧ�������� ENTERPRISEʱ����ҵ�ķ�������', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'LegalPerson'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�����', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'MinsettleAmount'
GO
EXEC sp_addextendedproperty N'MS_Description', N'��������', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'Riskreserveday'
GO
EXEC sp_addextendedproperty N'MS_Description', N'���п���', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BankAccountNumber'
GO
EXEC sp_addextendedproperty N'MS_Description', N'������', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BankName'
GO
EXEC sp_addextendedproperty N'MS_Description', N'������', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'AccountName'
GO
EXEC sp_addextendedproperty N'MS_Description', N'���п����  PrivateCash����˽ PublicCash�� �Թ�', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BankAccountType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'����ʡ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BankProvince'
GO
EXEC sp_addextendedproperty N'MS_Description', N'������', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BankCity'
GO
EXEC sp_addextendedproperty N'MS_Description', N'��������  N�������㣺 N - �����Զ��� Y - �����Զ�����Ҫͨ�����ֽӿڻ��̻���̨���ܽ��н��㡣  ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'ManualSettle'
GO
EXEC sp_addextendedproperty N'MS_Description', N'ǩ����Ϣ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'Hmac'
GO
EXEC sp_addextendedproperty N'MS_Description', N'ע��ʱ��', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'Addtime'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�ױ����˻����루���ױ����صģ�', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'Ledgerno'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�ױ����˻����', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BalanceRecord'
GO
