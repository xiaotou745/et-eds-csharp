--java版菜单
INSERT INTO AuthorityMenuClass(ParId,MenuName,BeLock,IsButton, JavaUrl) VALUES(10,'小费管理',0,0,'/ordertip/list')
INSERT INTO AuthorityMenuClass(ParId,MenuName,BeLock,IsButton, JavaUrl) VALUES(10,'任务配送费配置',0,0,'/taskdistributionconfig/list')


--切换java版菜单
UPDATE AuthorityMenuClass SET url=NULL WHERE id IN (14,15,16)
DELETE AuthorityMenuClass  WHERE Id=14
go

UPDATE dbo.AuthorityMenuClass SET AuthCode='SuperManManager_SuperManManager_ClienterAudit_Btn' WHERE Id=18--审核
UPDATE dbo.AuthorityMenuClass SET AuthCode='SuperManManager_SuperManManager_ClienterCancel_Btn' WHERE Id=19--审核
UPDATE dbo.AuthorityMenuClass SET AuthCode='SuperManManager_SuperManManager_ClienterBlanceChange_Btn' WHERE Id=51--余额调整
UPDATE dbo.AuthorityMenuClass SET AuthCode='SuperManManager_SuperManManager_ModifyInfo_Btn' WHERE Id=62--修改信息
UPDATE dbo.AuthorityMenuClass SET Url='' WHERE id=4--切换URL
go