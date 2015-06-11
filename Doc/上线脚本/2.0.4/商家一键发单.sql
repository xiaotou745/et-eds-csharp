--商家绑定、一键发单列
use superman
alter table dbo.business add IsBind smallint not null default 0; 
alter table dbo.business add OneKeyPubOrder int not null default 0; 
alter table dbo.clienter add IsBind smallint not null default 0; 
go




------------初始化北京、上海一键发单数据，countrow可能会变
declare @countrow INT
  --受影响行数
--set @countrow = 5433  --预计受影响行数
select  @countrow=COUNT(1) from dbo.business where City='北京市' or City='上海市'


begin transaction
update dbo.business set OneKeyPubOrder=1 where City!='北京市' or City!='上海市'
if ( @@error <> 0
     or @@rowcount <> @countrow
   ) 
    begin
        rollback
        print 'RollBack'
        return
    end

print 'Commit Start'
commit
print 'Commit Over'
