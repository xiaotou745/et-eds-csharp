/*==============================================================*/
/* Table: BusinessClienterRelation                                   */
/*==============================================================*/
create table dbo.BusinessClienterRelation (
   Id                   int                  identity(1, 1),
   BusinessId           int                  not null constraint DF_BusinessClienterRelation_BusinessId default (0),
   ClienterId           int                  not null constraint DF_BusinessClienterRelation_ClienterId default (0),
   IsEnable             smallint             not null constraint DF_BusinessClienterRelation_IsEnable default (1),
   CreateBy             nvarchar(50)         not null constraint DF_BusinessClienterRelation_CreateBy default (''),
   CreateTime           datetime             not null constraint DF_BusinessClienterRelation_CreateTime default getdate(),
   UpdateBy             nvarchar(50)         not null constraint DF_BusinessClienterRelation_UpdateBy default (''),
   UpdateTime           datetime             not null constraint DF_BusinessClienterRelation_UpdateTime default getdate(),
   IsBind               smallint             not null constraint DF_BusinessClienterRelation_IsBind default (1),
   constraint PK_BusinessClienterRelation primary key (Id)
         on "PRIMARY"
)
on "PRIMARY"
go


execute sp_addextendedproperty 'MS_Description', 
   '商户ID',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'BusinessId'
go


execute sp_addextendedproperty 'MS_Description', 
   '骑士ID',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'ClienterId'
go

execute sp_addextendedproperty 'MS_Description', 
   '是否有效(0:否 1:是)',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'IsEnable'
go

execute sp_addextendedproperty 'MS_Description', 
   '创建人',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'CreateBy'
go

execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'CreateTime'
go

execute sp_addextendedproperty 'MS_Description', 
   '最后一次更新人',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'UpdateBy'
go

execute sp_addextendedproperty 'MS_Description', 
   '最后一次更改时间',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'UpdateTime'
go

execute sp_addextendedproperty 'MS_Description', 
   '是否绑定(0:否 1:是)',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'IsBind'
go
