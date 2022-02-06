select b.budget_id,b.account_id, bc.category_id, bc.amount
into budget.budget_categories_temp
from budget.budget_categories bc
join budget.budgets b
	on b.id = bc.budget_id
go;

drop table budget.budget_categories
go;

drop table budget.budgets
go;

create table budget.budgets(
	budget_id int not null,
	account_id bigint not null foreign key references budget.accounts(id),
	[date] date not null
	primary key (budget_id, account_id)
)
go;

insert into budget.budgets
select * from budget.budget_temp
go;

drop table budget.budget_temp
go;

create table budget.budget_categories(
	budget_id int not null,
	account_id bigint not null,
	category_id bigint not null foreign key references budget.categories(id),
	amount decimal(10,2) not null ,
	primary key (budget_id, account_id, category_id),
	constraint budget_categories_fk
	foreign key (budget_id, account_id) references budget.budgets(budget_id, account_id)
)
go;

insert into budget.budget_categories
select * from budget.budget_categories_temp
go;

drop table budget.budget_categories_temp
go;