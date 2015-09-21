USE superman
go

update business SET GroupId=0 WHERE GroupId IS null
 
alter table business add constraint business_groupid default (0) for [GroupId]