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
   '�̻�ID',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'BusinessId'
go


execute sp_addextendedproperty 'MS_Description', 
   '��ʿID',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'ClienterId'
go

execute sp_addextendedproperty 'MS_Description', 
   '�Ƿ���Ч(0:�� 1:��)',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'IsEnable'
go

execute sp_addextendedproperty 'MS_Description', 
   '������',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'CreateBy'
go

execute sp_addextendedproperty 'MS_Description', 
   '����ʱ��',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'CreateTime'
go

execute sp_addextendedproperty 'MS_Description', 
   '���һ�θ�����',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'UpdateBy'
go

execute sp_addextendedproperty 'MS_Description', 
   '���һ�θ���ʱ��',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'UpdateTime'
go

execute sp_addextendedproperty 'MS_Description', 
   '�Ƿ��(0:�� 1:��)',
   'user', 'dbo', 'table', 'BusinessClienterRelation', 'column', 'IsBind'
go
