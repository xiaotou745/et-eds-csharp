ALTER TABLE [dbo].OrderChild ADD prepay_id varchar(200) not null default '' ;--΢��֧������΢�Ŷ�����
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'΢��֧������΢�Ŷ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderChild', @level2type=N'COLUMN',@level2name=N'prepay_id'






ALTER TABLE [dbo].BusinessRecharge ADD prepay_id varchar(200) not null default '' ;--΢��֧������΢�Ŷ�����
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'΢��֧������΢�Ŷ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessRecharge', @level2type=N'COLUMN',@level2name=N'prepay_id'
