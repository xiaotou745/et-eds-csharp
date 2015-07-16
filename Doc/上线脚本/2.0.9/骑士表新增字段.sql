USE [superman]
GO

ALTER TABLE [dbo].[clienter] ADD [Timespan] nvarchar(50) COLLATE Chinese_PRC_CI_AS NULL ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ê±¼ä´Á' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'clienter', @level2type=N'COLUMN',@level2name=N'Timespan'
GO