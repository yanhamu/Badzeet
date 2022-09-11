alter table budget.categories add [uid] uniqueidentifier not null default newid() 
go
alter table budget.payments add [uid] uniqueidentifier not null default newid()
go
alter table budget.budget_categories add [uid] uniqueidentifier not null default newid()
go
alter table budget.accounts add [uid] uniqueidentifier not null default newid()
go
alter table budget.budgets add [uid] uniqueidentifier not null default newid()
go
alter table budget.scheduled_payments add [uid] uniqueidentifier not null default newid()
go
alter table scheduler.payments add [uid] uniqueidentifier not null default newid()
go