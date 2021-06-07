create table budget.budget_categories(
	budget_id bigint foreign key references budget.budgets(id) not null,
	category_id bigint foreign key references budget.categories(id) not null,
	amount decimal(10,2) not null,
	primary key (budget_id, category_id)
)