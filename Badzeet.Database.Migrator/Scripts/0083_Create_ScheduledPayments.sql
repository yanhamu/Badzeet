 create table budget.scheduled_payments(
	id bigint identity(1,1) primary key,
	account_id bigint not null foreign key references budget.accounts(id),
	[date] date not null,
	amount decimal(10,2) not null,
	[description] nvarchar(100) not null,
	category_id bigint not null foreign key references budget.categories(id),
	owner_id uniqueidentifier not null foreign key references budget.users(id)
)