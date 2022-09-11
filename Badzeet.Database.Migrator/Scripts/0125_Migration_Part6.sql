create function dbo.get_id(@id bigint)
returns uniqueidentifier
begin
	declare @i uniqueidentifier;
	select top 1 @i = [uid] from budget.accounts where @id = id
	return @i
end
go

select id, budget_id, dbo.get_id(account_id) as account_id, category_id, amount
into budget.bc
from budget.budget_categories
go

select uid as id, budget_id, dbo.get_id(account_id) as account_id, date
into budget.b
from budget.budgets
go

select id, dbo.get_id(account_id) as account_id, [name], [order]
into budget.c
from budget.categories
go

drop table budget.invitations
go

select id, dbo.get_id(account_id) as account_id, [date], amount, [description], category_id, owner_id, [type]
into budget.p
from budget.payments
go

select id, dbo.get_id(account_id) as account_id, [date], amount, [description], category_id, owner_id
into budget.sp
from budget.scheduled_payments
go

select [user_id], dbo.get_id(account_id) as account_id
into budget.ua
from budget.user_accounts
go

select id, dbo.get_id(account_id) as account_id, amount, [description], category_id, owner_id, updated_at, scheduled_at, scheduling_type, scheduling_metadata
into scheduler.p
from scheduler.payments
go

select uid as id, first_day_of_budget, created_at
into budget.a
from budget.accounts
go

drop table budget.budget_categories
go

drop table budget.payments
go

drop table budget.scheduled_payments
go

drop table budget.categories
go

drop table budget.user_accounts
go

drop table budget.budgets
go

drop table scheduler.payments
go

drop table budget.accounts
go

create table budget.accounts(
	id uniqueidentifier primary key,
	first_day_of_budget tinyint not null,
	created_at date not null
)
go

insert into budget.accounts
select a.id, a.first_day_of_budget, a.created_at
from budget.a
go

drop table budget.a
go

create table budget.budgets(
	id uniqueidentifier primary key,
	budget_id int not null,
	account_id uniqueidentifier foreign key references budget.accounts(id) not null,
	[date] date not null
)
go

insert into budget.budgets
select id, budget_id, account_id, [date]
from budget.b
go

drop table budget.b
go

create table budget.categories(
id uniqueidentifier primary key,
account_id uniqueidentifier not null foreign key references budget.accounts(id),
[name] nvarchar(100) not null,
[order] int not null
)
go

insert into budget.categories
select * from budget.c
go

drop table budget.c
go

create table budget.budget_categories(
id uniqueidentifier primary key,
budget_id int not null,
account_id uniqueidentifier not null foreign key references budget.accounts(id),
category_id uniqueidentifier not null foreign key references budget.categories(id),
amount decimal(10,2) not null
)
go

insert into budget.budget_categories
select * 
from budget.bc
go

drop table budget.bc
go

create table budget.payments(
id uniqueidentifier primary key, 
account_id uniqueidentifier not null foreign key references budget.accounts(id), 
[date] date not null, 
amount decimal(10,2) not null, 
[description] nvarchar(100) not null, 
category_id uniqueidentifier not null foreign key references budget.categories(id), 
owner_id uniqueidentifier not null foreign key references budget.users(id), 
[type] tinyint not null
)
go

insert into budget.payments
select * from budget.p
go

drop table budget.p
go

create table budget.scheduled_payments(
id uniqueidentifier primary key,
account_id uniqueidentifier not null foreign key references budget.accounts(id),
[date] date not null,
amount decimal(10,2) not null,
[description] nvarchar(100) not null,
category_id uniqueidentifier not null foreign key references budget.categories(id),
owner_id uniqueidentifier not null foreign key references budget.users(id)
)
go

insert into budget.scheduled_payments
select * from budget.sp
go

drop table budget.sp
go

create table budget.user_accounts(
[user_id] uniqueidentifier not null,
[account_id] uniqueidentifier not null,
primary key(user_id, account_id)
)
go

insert into budget.user_accounts
select * from budget.ua
go

drop table budget.ua
go

create table scheduler.payments(
id uniqueidentifier primary key, 
account_id uniqueidentifier not null, 
amount decimal(10,2) not null, 
[description] nvarchar(100) not null, 
category_id uniqueidentifier not null, 
owner_id uniqueidentifier not null, 
updated_at datetime2(0) not null, 
scheduled_at datetime2(0) not null, 
scheduling_type tinyint not null, 
scheduling_metadata nvarchar(400)
)
go

drop table scheduler.p
go