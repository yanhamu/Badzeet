create schema budget
go

create table budget.users(
id uniqueidentifier primary key,
nickname nvarchar(100) not null)
go

create table budget.accounts(
id bigint identity(1,1) primary key,
first_day_of_budget tinyint not null,
created_at date not null
)
go

create table budget.user_accounts(
[user_id] uniqueidentifier not null foreign key references budget.users(id),
account_id bigint not null foreign key references budget.accounts(id)
)
go 

create table budget.categories(
id bigint identity(1,1) primary key,
account_id bigint foreign key references budget.accounts(id),
[name] nvarchar(100) not null,
[order] int not null
)
go

create table budget.transactions(
id bigint identity(1,1) not null,
account_id bigint not null,
[date] date not null,
amount decimal(10,2) not null,
[description] nvarchar(100) not null,
category_id bigint not null foreign key references budget.categories(id),
owner_id uniqueidentifier not null foreign key references budget.users(id)
)
go

create table budget.invitations(
id uniqueidentifier primary key,
account_id bigint not null foreign key references budget.accounts(id),
owner_id uniqueidentifier foreign key references budget.users(id),
used_at datetime2(2),
created_at datetime2(2) not null
)
go