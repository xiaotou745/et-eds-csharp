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
   '骑士app消息表',
   'user', @CurrentUser, 'table', 'ClienterMessage'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '自增ID(PK)',
   'user', @CurrentUser, 'table', 'ClienterMessage', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '骑士Id',
   'user', @CurrentUser, 'table', 'ClienterMessage', 'column', 'ClienterId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '消息体',
   'user', @CurrentUser, 'table', 'ClienterMessage', 'column', 'Content'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否已读 0未读 1 已读',
   'user', @CurrentUser, 'table', 'ClienterMessage', 'column', 'IsRead'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '推送时间',
   'user', @CurrentUser, 'table', 'ClienterMessage', 'column', 'PubDate'
go
