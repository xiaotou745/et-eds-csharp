USE [superman]
GO
/****** Object:  Table [dbo].[LineHistory]    Script Date: 02/17/2016 14:31:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
if exists ( select * 
 from  sysobjects
 where name = 'LineHistory'
 and type = 'U')
 drop table LineHistory
go
CREATE TABLE [dbo].[LineHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DevPlatform] [nvarchar](100) NOT NULL,
	[OnLineProduct] [nvarchar](200) NOT NULL,
	[DevVersion] [nvarchar](20) NOT NULL,
	[OnLineTime] [datetime] NOT NULL,
	[OnLineContent] [nvarchar](2000) NOT NULL,
	[Remark] [nvarchar](200) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateName] [nvarchar](50) NOT NULL,
	[ModifyTime] [datetime] NULL,
	[ModifyName] [nvarchar](50) NULL,
	[IsEnable] [int] NOT NULL,
	[DevPlatformCode] [int] NOT NULL,
	[OnLineProductCode] [int] NOT NULL,
 CONSTRAINT [PK_LineHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属平台' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'DevPlatform'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上线产品' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'OnLineProduct'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'DevVersion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上线时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'OnLineTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上线内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'OnLineContent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'CreateName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'ModifyTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'ModifyName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属平台编号(DevPlatformType枚举)1.易代送管理后台,2.E代送 3.易代送智能调度 11人人推管理后台 12人人推App' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'DevPlatformCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品编码(OnLineProductType枚举):1 E代送管理后台,2 E代送骑士,3 商家中心,4 E代送（里程计费）,5 E代送商户 6.E代送智能调度 7.E代送轻骑士 11.人人推管理后台 12.人人推App ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineHistory', @level2type=N'COLUMN',@level2name=N'OnLineProductCode'
GO
/****** Object:  Default [DF_LineHistory_DevPlatform]    Script Date: 02/17/2016 14:31:40 ******/
ALTER TABLE [dbo].[LineHistory] ADD  CONSTRAINT [DF_LineHistory_DevPlatform]  DEFAULT ('') FOR [DevPlatform]
GO
/****** Object:  Default [DF_LineHistory_LineProduct]    Script Date: 02/17/2016 14:31:40 ******/
ALTER TABLE [dbo].[LineHistory] ADD  CONSTRAINT [DF_LineHistory_LineProduct]  DEFAULT ('') FOR [OnLineProduct]
GO
/****** Object:  Default [DF_LineHistory_DevVersion]    Script Date: 02/17/2016 14:31:40 ******/
ALTER TABLE [dbo].[LineHistory] ADD  CONSTRAINT [DF_LineHistory_DevVersion]  DEFAULT ('') FOR [DevVersion]
GO
/****** Object:  Default [DF_LineHistory_OnLineDate]    Script Date: 02/17/2016 14:31:40 ******/
ALTER TABLE [dbo].[LineHistory] ADD  CONSTRAINT [DF_LineHistory_OnLineDate]  DEFAULT ('') FOR [OnLineTime]
GO
/****** Object:  Default [DF_LineHistory_OnLineContent]    Script Date: 02/17/2016 14:31:40 ******/
ALTER TABLE [dbo].[LineHistory] ADD  CONSTRAINT [DF_LineHistory_OnLineContent]  DEFAULT ('') FOR [OnLineContent]
GO
/****** Object:  Default [DF_LineHistory_Remark]    Script Date: 02/17/2016 14:31:40 ******/
ALTER TABLE [dbo].[LineHistory] ADD  CONSTRAINT [DF_LineHistory_Remark]  DEFAULT ('') FOR [Remark]
GO
/****** Object:  Default [DF_LineHistory_CreateTime]    Script Date: 02/17/2016 14:31:40 ******/
ALTER TABLE [dbo].[LineHistory] ADD  CONSTRAINT [DF_LineHistory_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
/****** Object:  Default [DF_LineHistory_CreateName]    Script Date: 02/17/2016 14:31:40 ******/
ALTER TABLE [dbo].[LineHistory] ADD  CONSTRAINT [DF_LineHistory_CreateName]  DEFAULT ('') FOR [CreateName]
GO
/****** Object:  Default [DF__LineHisto__IsEna__7E97B1A9]    Script Date: 02/17/2016 14:31:40 ******/
ALTER TABLE [dbo].[LineHistory] ADD  CONSTRAINT [DF__LineHisto__IsEna__7E97B1A9]  DEFAULT ((1)) FOR [IsEnable]
GO
/****** Object:  Default [DF_LineHistory_DevPlatformCode]    Script Date: 02/17/2016 14:31:40 ******/
ALTER TABLE [dbo].[LineHistory] ADD  CONSTRAINT [DF_LineHistory_DevPlatformCode]  DEFAULT ((0)) FOR [DevPlatformCode]
GO
/****** Object:  Default [DF_LineHistory_OnLineProductCode]    Script Date: 02/17/2016 14:31:40 ******/
ALTER TABLE [dbo].[LineHistory] ADD  CONSTRAINT [DF_LineHistory_OnLineProductCode]  DEFAULT ((0)) FOR [OnLineProductCode]
GO
