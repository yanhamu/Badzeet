create table budget.budgets(
	id bigint identity(1,1) primary key,
	account_id bigint foreign key references budget.accounts(id) not null,
	[date] date not null
)