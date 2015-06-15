if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessMessage')
            and   type = 'U')
   drop table BusinessMessage
go

/*==============================================================*/
/* Table: BusinessMessage                                       */
/*==============================================================*/
create table BusinessMessage (
   Id                   bigint               identity,
   BusinessId           int                  not null,
   Content              nvarchar(1024)       not null,
   IsRead               smallint             not null default 0,
   PubDate              datetime             not null default getdate()
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�̻�app��Ϣ��',
   'user', @CurrentUser, 'table', 'BusinessMessage'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����ID(PK)',
   'user', @CurrentUser, 'table', 'BusinessMessage', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�̻�id',
   'user', @CurrentUser, 'table', 'BusinessMessage', 'column', 'BusinessId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��Ϣ��',
   'user', @CurrentUser, 'table', 'BusinessMessage', 'column', 'Content'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�Ƿ��Ѷ� 0δ�� 1 �Ѷ�',
   'user', @CurrentUser, 'table', 'BusinessMessage', 'column', 'IsRead'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����ʱ��',
   'user', @CurrentUser, 'table', 'BusinessMessage', 'column', 'PubDate'
go
