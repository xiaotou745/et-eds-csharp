ALTER TABLE Clienter
ADD DeliveryCompanyId int NOT NULL DEFAULT 0
GO 
--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
 @value=N'����������˾ID' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Clienter', 
  @level2type=N'COLUMN',
  @level2name=N'DeliveryCompanyId'
GO

--����ʱ �������еľ�����Ϊ0
UPDATE Clienter SET DeliveryCompanyId=0
