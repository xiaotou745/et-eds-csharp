if exists (select 1
            from  sysobjects
           where  id = object_id('ClienterMessage')
            and   type = 'U')
   drop table ClienterMessage
go

/*==============================================================*/
/* Table: ClienterMessage                                       */
/*==============================================================*/
create table ClienterMessage (
   Id                   bigint               identity,
   ClienterId           int                  not null,
   Content              nvarchar(1024)       not null,
   IsRead               smallint             not null default 0,
   PubDate              datetime             not null default getdate()
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��ʿapp��Ϣ��',
   'user', @CurrentUser, 'table', 'ClienterMessage'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����ID(PK)',
   'user', @CurrentUser, 'table', 'ClienterMessage', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��ʿId',
   'user', @CurrentUser, 'table', 'ClienterMessage', 'column', 'ClienterId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��Ϣ��',
   'user', @CurrentUser, 'table', 'ClienterMessage', 'column', 'Content'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�Ƿ��Ѷ� 0δ�� 1 �Ѷ�',
   'user', @CurrentUser, 'table', 'ClienterMessage', 'column', 'IsRead'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����ʱ��',
   'user', @CurrentUser, 'table', 'ClienterMessage', 'column', 'PubDate'
go
