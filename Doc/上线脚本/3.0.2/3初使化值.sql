--java��˵�
INSERT INTO AuthorityMenuClass(ParId,MenuName,BeLock,IsButton, JavaUrl) VALUES(10,'С�ѹ���',0,0,'/ordertip/list')
INSERT INTO AuthorityMenuClass(ParId,MenuName,BeLock,IsButton, JavaUrl) VALUES(10,'�������ͷ�����',0,0,'/taskdistributionconfig/list')


--�л�java��˵�
UPDATE AuthorityMenuClass SET url=NULL WHERE id IN (14,15,16)
DELETE AuthorityMenuClass  WHERE Id=14
go

UPDATE dbo.AuthorityMenuClass SET AuthCode='SuperManManager_SuperManManager_ClienterAudit_Btn' WHERE Id=18--���
UPDATE dbo.AuthorityMenuClass SET AuthCode='SuperManManager_SuperManManager_ClienterCancel_Btn' WHERE Id=19--���
UPDATE dbo.AuthorityMenuClass SET AuthCode='SuperManManager_SuperManManager_ClienterBlanceChange_Btn' WHERE Id=51--������
UPDATE dbo.AuthorityMenuClass SET AuthCode='SuperManManager_SuperManManager_ModifyInfo_Btn' WHERE Id=62--�޸���Ϣ
UPDATE dbo.AuthorityMenuClass SET Url='' WHERE id=4--�л�URL
go