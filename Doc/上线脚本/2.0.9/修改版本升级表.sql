USE [superman]
GO

/****** Object:  Table [dbo].[AppVersion]    Script Date: 07/16/2015 08:57:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF((select COUNT(1) from sysobjects where id = object_id('AppVersion') and type = 'u')>0)
BEGIN
	DROP TABLE AppVersion
END
CREATE TABLE [dbo].[AppVersion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Version] [nvarchar](20) NOT NULL,
	[IsMust] [int] NULL,
	[UpdateUrl] [nvarchar](100) NULL,
	[Message] [nvarchar](500) NULL,
	[PlatForm] [tinyint] NOT NULL,
	[UserType] [tinyint] NOT NULL,
	[CreateDate] [datetime] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[IsTiming] [int] NULL,
	[TimingDate] [datetime] NULL,
	[PubStatus] [int] NOT NULL,
 CONSTRAINT [PK_AppVersion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ǰ�汾��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'Version'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�ǿ������ 1 �� 0�� Ĭ��0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'IsMust'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ص�ַ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'UpdateUrl'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������Ϣ ���Բ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'Message'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ͻ������� 1:Android 2 :IOS Ĭ��Android' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'PlatForm'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û��汾 1 ��ʿ 2 �̼� Ĭ��1��ʿ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'UserType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'UpdateDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�ʱ 0�� 1 ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'IsTiming'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʱ����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'TimingDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����״̬ 0������ 1 �ѷ��� 2 ȡ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'PubStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'App�汾���Ʊ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion'
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_IsMust]  DEFAULT ((0)) FOR [IsMust]
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_PlatForm]  DEFAULT ((1)) FOR [PlatForm]
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_UserType]  DEFAULT ((1)) FOR [UserType]
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[AppVersion] ADD  DEFAULT (getdate()) FOR [UpdateDate]
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_IsTiming]  DEFAULT ((0)) FOR [IsTiming]
GO

ALTER TABLE [dbo].[AppVersion] ADD  DEFAULT ((0)) FOR [PubStatus]
GO


SELECT * FROM dbo.AppVersion


