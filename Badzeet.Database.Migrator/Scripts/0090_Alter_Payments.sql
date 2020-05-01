alter table budget.payments add [type] tinyint
go
update budget.payments set type = 1
go
alter table budget.payments alter column [type] tinyint not null