select id, account_id, [date], amount, [description], dbo.get_id(category_id) as category_id, owner_id, [uid]
into budget.sp
from [budget].[scheduled_payments]
go

select *
into budget.c
from budget.categories
go

drop table budget.categories
go

create table budget.categories(
	id uniqueidentifier primary key, 
	account_id bigint not null foreign key references budget.accounts(id), 
	[name] nvarchar(100) not null, 
	[order] int not null
)
go

insert into budget.categories(id, account_id, [name], [order])
select [uid], account_id, [name], [order]
from budget.c
go

drop table budget.c
go

drop table scheduler.payments
go

create table scheduler.payments(
	id uniqueidentifier primary key, 
	account_id bigint not null, 
	amount decimal(10,2) not null, 
	[description] nvarchar(100) not null, 
	category_id uniqueidentifier not null, 
	owner_id uniqueidentifier not null, 
	updated_at datetime2(0) not null, 
	scheduled_at datetime2(0) null, 
	scheduling_type tinyint not null, 
	scheduling_metadata nvarchar(400) null
)

insert into scheduler.payments
select 	id, account_id, amount, [description], category_id, owner_id, updated_at, scheduled_at, scheduling_type, scheduling_metadata
from scheduler.p
go

drop table scheduler.p
go