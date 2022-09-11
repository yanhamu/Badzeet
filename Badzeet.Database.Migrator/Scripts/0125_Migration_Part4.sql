drop table budget.budget_categories
go

create table budget.budget_categories(
	id uniqueidentifier primary key,
	budget_id int not null,
	account_id bigint not null,
	category_id uniqueidentifier not null foreign key references budget.categories(id), 
	amount decimal(10,2) not null,
	constraint fk_budgets foreign key (budget_id, account_id) references budget.budgets(budget_id, account_id)
)
go

insert into budget.budget_categories(id, budget_id, category_id, account_id, amount) 
select id, budget_id, category_id, account_id, amount
from [budget].[bc]
go

drop table budget.bc
go

drop table budget.scheduled_payments
go

create table budget.scheduled_payments(
	id uniqueidentifier not null primary key, 
	account_id bigint not null foreign key references budget.accounts(id), 
	[date] date not null, 
	amount decimal(10,2) not null,
	[description] nvarchar(100) not null, 
	category_id uniqueidentifier not null foreign key references budget.categories(id), 
	owner_id uniqueidentifier not null foreign key references budget.users(id)
)
go

insert into budget.scheduled_payments(id, account_id, [date], amount, [description], category_id, owner_id)
select uid, account_id, date, amount, description, category_id, owner_id
from budget.sp
go

drop table budget.sp
go
