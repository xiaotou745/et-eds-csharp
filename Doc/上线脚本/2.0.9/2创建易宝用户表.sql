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
EXEC sp_addextendedproperty N'MS_Description', N'易宝子账户表
   ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Id，自增', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'Id'
GO
EXEC sp_addextendedproperty N'MS_Description', N'用户id （骑士id/商户id）', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'UserId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'用户类型（0骑士 1商家  默认 0）', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'UserType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'注册易宝账户时的请求id  注册请求号 在主帐号下唯一 MAX(50 )', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'RequestId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'商户编号 易代送公司主账号 ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'CustomerNumberr'
GO
EXEC sp_addextendedproperty N'MS_Description', N'商户密钥', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'HmacKey'
GO
EXEC sp_addextendedproperty N'MS_Description', N'绑定手机', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BindMobile'
GO
EXEC sp_addextendedproperty N'MS_Description', N'注册类型  PERSON ：个人 ENTERPRISE：企业 ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'CustomerType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'签约名   商户签约名；个人，填写姓名；企业，填写企业名称。', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'SignedName'
GO
EXEC sp_addextendedproperty N'MS_Description', N'联系人', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'LinkMan'
GO
EXEC sp_addextendedproperty N'MS_Description', N'身份证  customertype为PERSON时，必填', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'IdCard'
GO
EXEC sp_addextendedproperty N'MS_Description', N'营业执照号 customertype为ENTERPRISE时，必填', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BusinessLicence'
GO
EXEC sp_addextendedproperty N'MS_Description', N'姓名  PERSON时，idcard对应的姓名； ENTERPRISE时，企业的法人姓名', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'LegalPerson'
GO
EXEC sp_addextendedproperty N'MS_Description', N'起结金额', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'MinsettleAmount'
GO
EXEC sp_addextendedproperty N'MS_Description', N'结算周期', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'Riskreserveday'
GO
EXEC sp_addextendedproperty N'MS_Description', N'银行卡号', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BankAccountNumber'
GO
EXEC sp_addextendedproperty N'MS_Description', N'开户行', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BankName'
GO
EXEC sp_addextendedproperty N'MS_Description', N'开户名', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'AccountName'
GO
EXEC sp_addextendedproperty N'MS_Description', N'银行卡类别  PrivateCash：对私 PublicCash： 对公', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BankAccountType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'开户省', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BankProvince'
GO
EXEC sp_addextendedproperty N'MS_Description', N'开户市', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BankCity'
GO
EXEC sp_addextendedproperty N'MS_Description', N'自助结算  N自助结算： N - 隔天自动打款； Y - 不会自动打款，需要通过提现接口或商户后台功能进行结算。  ', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'ManualSettle'
GO
EXEC sp_addextendedproperty N'MS_Description', N'签名信息', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'Hmac'
GO
EXEC sp_addextendedproperty N'MS_Description', N'注册时间', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'Addtime'
GO
EXEC sp_addextendedproperty N'MS_Description', N'易宝子账户编码（由易宝返回的）', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'Ledgerno'
GO
EXEC sp_addextendedproperty N'MS_Description', N'易宝内账户余额', 'SCHEMA', N'dbo', 'TABLE', N'YeePayUser', 'COLUMN', N'BalanceRecord'
GO
