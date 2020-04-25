create schema scheduler

create table scheduler.payments (
	id bigint identity(1,1) primary key,
	account_id bigint not null,
	[date] date not null,
	amount decimal(10,2) not null,
	[description] nvarchar(100) not null,
	category_id bigint not null,
	owner_id uniqueidentifier not null,
	updated_at datetime2(0) not null,
	scheduled_at datetime2(0) null,
	scheduling_type tinyint not null,
	scheduling_metadata nvarchar(400) null
)