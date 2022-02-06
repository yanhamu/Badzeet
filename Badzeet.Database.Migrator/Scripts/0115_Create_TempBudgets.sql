create table budget.budget_temp(
	budget_id int not null,
	account_id bigint not null foreign key references budget.accounts(id),
	[date] date not null
	primary key (budget_id, account_id)
)
go;

insert into budget.budget_temp
select budget_id, account_id, date from budget.budgets
