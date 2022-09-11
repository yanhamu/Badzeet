alter table [budget].[scheduled_payments] drop constraint [fk_scheduled_categories]
go

alter table [budget].[payments] drop constraint [fk_payments_categories]
go

alter table [budget].[budget_categories] drop constraint [budget_categories_fk]
go

create function get_id(@categoryId bigint) returns uniqueidentifier
begin
	declare @g uniqueidentifier
	select top 1 @g = [uid] from budget.categories where id = @categoryId
	return @g
end
go 

select [uid] as id, budget_id, dbo.get_id(category_id) as category_id, account_id, amount
into budget.bc
from budget.budget_categories
go 

select [uid] as id, account_id, amount, [description], dbo.get_id(category_id) as category_id, owner_id, updated_at, scheduled_at, scheduling_type, scheduling_metadata
into scheduler.p
from [scheduler].[payments]
go