use  superman
go
--------------------------------------������
 alter table dbo.[order] add ReceiveCode varchar(16) not null  DEFAULT '';
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�ջ���' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'order', 
  @level2type=N'COLUMN',
  @level2name=N'ReceiveCode'
  go  

  
   alter table dbo.[order] add PubCity nvarchar(45) not null  DEFAULT '';
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'��������' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'order', 
  @level2type=N'COLUMN',
  @level2name=N'PubCity'
  go 
  
   alter table dbo.[order] add IsReceiveCode int not null  DEFAULT 0;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�Ƿ���ȡ��֤��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'order', 
  @level2type=N'COLUMN',
  @level2name=N'IsReceiveCode'
  go 
  
  
--------------------------------------OrderOther
alter table dbo.OrderOther
add IsOrderRemind int default 0
go
alter table dbo.OrderOther
add OrderRemindTime datetime null
go
exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�ߵ�Ĭ��0û��1��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderOther', @level2type=N'COLUMN',@level2name=N'IsOrderRemind'
go
exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ե��ߵ�ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderOther', @level2type=N'COLUMN',@level2name=N'OrderRemindTime'
go
update dbo.OrderOther
set IsOrderRemind=0;
go

--------------------------------------���������������
 alter table dbo.clienter add PushShanSongOrderSet int not null  default 1;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'��̼�����ʿ���Ƶ�����  0���Ƶ� 1�Ƶ� Ĭ��1' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'clienter', 
  @level2type=N'COLUMN',
  @level2name=N'PushShanSongOrderSet'  

---------------------------------�������ͷ�
 alter table TaskDistributionConfig add Steps int not null  default 0;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�Ƽ۽���' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'TaskDistributionConfig', 
  @level2type=N'COLUMN',
  @level2name=N'Steps'
go 
  
   alter table TaskDistributionConfig add   Remark NVARCHAR(1000) not null  default '';
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'��ע' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'TaskDistributionConfig', 
  @level2type=N'COLUMN',
  @level2name=N'Remark'
go