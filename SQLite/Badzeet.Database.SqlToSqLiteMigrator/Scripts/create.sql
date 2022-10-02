create table accounts(
	id integer primary key autoincrement,
	first_day_of_budget tinyint not null,
	created_at date not null
);

create table users(
	id uniqueidentifier primary key,
	nickname nvarchar(100) not null
);

create table user_accounts(
	user_id uniqueidentifier not null,
	account_id integer not null,
	primary key(user_id, account_id),
	foreign key (user_id) references users(id),
	foreign key (account_id) references accounts(id)
);

create table budgets(
	id uniqueidentifier primary key,
	budget_id int not null,
	account_id integer not null,
	date date not null,
	foreign key (account_id) references accounts(id)
);

create table categories(
	id integer primary key autoincrement,
	account_id integer not null,
	name nvarchar(100) not null,
	[order] int not null,
	foreign key (account_id) references accounts(id)
);

create table budget_categories(
	id uniqueidentifier primary key,
	budget_id int not null,
	account_id integer not null,
	category_id integer not null,
	amount decimal(10,2) not null,
	foreign key (account_id) references accounts(id),
	foreign key (category_id) references categories(id)
);

create table payments(
	id int primary key,
	account_id integer not null,
	category_id integer not null,
	[date] date not null,
	amount decimal(10,2) not null,
	description nvarchar(100) not null,
	owner_id uniqueidentifier not null,
	type tinyint not null,
	foreign key (account_id) references accounts(id),
	foreign key (owner_id) references users(id),
	foreign key (category_id) references categories(id)
);

create table scheduled_payments(
	id uniqueidentifier primary key,
	account_id integer not null,
	[date] date not null,
	amount decimal(10,2) not null,
	description nvarchar(100) not null,
	category_id integer not null,
	owner_id uniqueidentifier not null,
	foreign key (account_id) references accounts(id),
	foreign key (category_id) references categories(id),
	foreign key (owner_id) references users(id)
);

create table sch_payments(
	id uniqueidentifier primary key,
	account_id integer not null,
	amount decimal(10,2) not null,
	description nvarchar(100) not null,
	category_id integer not null,
	owner_id uniqueidentifier not null,
	updated_at datetime not null,
	scheduled_at datetime not null,
	scheduling_type tinyint not null,
	metadata nvarchar(400) not null
);

create table u_users(
	id uniqueidentifier primary key,
	username nvarchar(100) not null,
	password nvarchar(200) not null
);