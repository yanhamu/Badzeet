create table budget.payments(
	id bigint identity(1,1) primary key,
	account_id bigint not null foreign key references budget.accounts(id),
	[date] date not null,
	amount decimal(10,2) not null,
	[description] nvarchar(100) not null,
	category_id bigint not null foreign key references budget.categories(id),
	owner_id uniqueidentifier not null foreign key references budget.users(id)
)

set identity_insert budget.payments on 

insert into budget.payments(id, account_id, date, amount, description, category_id, owner_id)
select id, account_id, date, amount, description, category_id, owner_id
from budget.transactions

set identity_insert budget.payments off

drop table budget.transactions