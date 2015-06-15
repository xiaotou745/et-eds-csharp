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
   '商户app消息表',
   'user', @CurrentUser, 'table', 'BusinessMessage'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '自增ID(PK)',
   'user', @CurrentUser, 'table', 'BusinessMessage', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '商户id',
   'user', @CurrentUser, 'table', 'BusinessMessage', 'column', 'BusinessId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '消息体',
   'user', @CurrentUser, 'table', 'BusinessMessage', 'column', 'Content'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否已读 0未读 1 已读',
   'user', @CurrentUser, 'table', 'BusinessMessage', 'column', 'IsRead'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '推送时间',
   'user', @CurrentUser, 'table', 'BusinessMessage', 'column', 'PubDate'
go
