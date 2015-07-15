USE superman
GO
ALTER TABLE dbo.AppVersion
ADD CreateBy nvarchar(50) NULL--��Ӵ�����
GO
ALTER TABLE dbo.AppVersion
ADD UpdateDate datetime NOT NULL DEFAULT(GETDATE()) --��Ӹ���ʱ��
GO 
ALTER TABLE dbo.AppVersion
ADD UpdateBy nvarchar(50) NULL --��Ӹ�����
GO 
ALTER TABLE dbo.AppVersion
ADD IsTiming bit NOT NULL DEFAULT(0) --�Ƿ�ʱ 0�� 1 ��
GO 
ALTER TABLE dbo.AppVersion
ADD TimingDate datetime NULL --��ʱ����ʱ��
GO 
ALTER TABLE dbo.AppVersion
ADD PubStatus INT NOT  NULL DEFAULT(0) --����״̬
GO 


--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'����״̬ 0������ 1 �ѷ��� 2 ȡ������' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AppVersion', 
  @level2type=N'COLUMN',
  @level2name=N'PubStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'������' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AppVersion', 
  @level2type=N'COLUMN',
  @level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'����ʱ��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AppVersion', 
  @level2type=N'COLUMN',
  @level2name=N'UpdateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'������' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AppVersion', 
  @level2type=N'COLUMN',
  @level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�Ƿ�ʱ 0�� 1 ��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AppVersion', 
  @level2type=N'COLUMN',
  @level2name=N'IsTiming'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'��ʱ����ʱ��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AppVersion', 
  @level2type=N'COLUMN',
  @level2name=N'TimingDate'
GO

