alter table dbo.BusinessWithdrawForm add DealStatus smallint not null default 0 ;--����״̬��0�������� 1���Ѵ���
alter table dbo.BusinessWithdrawForm add DealCount int not null default 0 ;--�������
alter table dbo.ClienterWithdrawForm add DealStatus smallint not null default 0 ;--����״̬��0�������� 1���Ѵ���
alter table dbo.ClienterWithdrawForm add DealCount int not null default 0 ;--�������
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����״̬��0�������� 1���Ѵ���' ,
 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessWithdrawForm', @level2type=N'COLUMN',@level2name=N'DealStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������' ,
 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessWithdrawForm', @level2type=N'COLUMN',@level2name=N'DealCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����״̬��0�������� 1���Ѵ���' ,
 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterWithdrawForm', @level2type=N'COLUMN',@level2name=N'DealStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������' ,
 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterWithdrawForm', @level2type=N'COLUMN',@level2name=N'DealCount'
GO