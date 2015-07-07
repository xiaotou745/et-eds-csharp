
ALTER TABLE Business
ADD RecommendPhone NVARCHAR(40)  NULL 
GO 
--添加列注释
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
 @value=N'商家推荐人手机号' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Business', 
  @level2type=N'COLUMN',
  @level2name=N'RecommendPhone'
GO
