drop table budget.payments
go

create table budget.payments(
	id uniqueidentifier primary key, 
	account_id bigint not null foreign key references budget.accounts(id), 
	[date] date not null, 
	amount decimal(10,2) not null, 
	[description] nvarchar(100) not null, 
	category_id uniqueidentifier not null foreign key references budget.categories(id), 
	owner_id uniqueidentifier not null foreign key references budget.users(id), 
	[type] tinyint not null
)
go

insert into budget.payments(id, account_id, date, amount, description, category_id, owner_id, type)
select uid, account_id, date, amount, description, category_id, owner_id, type
from budget.p
go

drop table budget.p
go

drop function dbo.get_id
go