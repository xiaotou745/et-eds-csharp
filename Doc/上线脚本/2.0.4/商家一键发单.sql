--�̼Ұ󶨡�һ��������
use superman
alter table dbo.business add IsBind smallint not null default 0; 
alter table dbo.business add OneKeyPubOrder int not null default 0; 
alter table dbo.clienter add IsBind smallint not null default 0; 
go




------------��ʼ���������Ϻ�һ���������ݣ�countrow���ܻ��
declare @countrow INT
  --��Ӱ������
--set @countrow = 5433  --Ԥ����Ӱ������
select  @countrow=COUNT(1) from dbo.business where City='������' or City='�Ϻ���'


begin transaction
update dbo.business set OneKeyPubOrder=1 where City!='������' or City!='�Ϻ���'
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
