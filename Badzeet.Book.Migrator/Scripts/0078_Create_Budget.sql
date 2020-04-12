create table budget.category_budget(
	account_id bigint foreign key references budget.accounts(id),
	id smallint,
	category_id bigint foreign key references budget.categories(id),
	amount decimal(10,2) not null,
	primary key (account_id, id, category_id)
)