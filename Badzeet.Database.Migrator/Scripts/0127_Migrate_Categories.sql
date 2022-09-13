alter table budget.payments add category nvarchar(100) null
go

create function get_category(@category_id uniqueidentifier)
returns nvarchar(100)
begin
declare @c nvarchar(100)
select top 1 @c = name from budget.categories where id = @category_id
return @c
end
go

update budget.payments set category = dbo.get_category(category_id)
go

drop function dbo.get_category
go

alter table budget.payments alter column
category nvarchar(100) not null
go