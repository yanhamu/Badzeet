create function get_first_date(@account_id bigint)
returns date
begin 
	declare @first_day tinyint
	select @first_day = first_day_of_budget from budget.accounts where id = @account_id
	return '2000-01-' + convert(nvarchar(2),@first_day)
end
go

create function get_date(@account_id bigint, @i int)
returns date
begin
	declare @start_date date = dbo.get_first_date(@account_id)
	declare @c int = 0

	while @c < @i
	begin
		set @start_date = DATEADD(month, 1, @start_date)
		set @c = @c + 1;
	end
	return @start_date
end
go

declare c cursor for 
	select distinct account_id, id
	from budget.category_budget
	order by id asc

declare 
@account_id bigint,
@id int,
@date date,
@budget_id bigint

open c
fetch next from c into @account_id, @id

WHILE @@FETCH_STATUS = 0  
begin
	set @date = dbo.get_date(@account_id , @id)
	insert into budget.budgets values (@account_id, @date)
	set @budget_id = SCOPE_IDENTITY()
	
	insert into budget.budget_categories(budget_id, category_id, amount)
	select @budget_id, category_id, amount 
	from budget.category_budget
	where account_id = @account_id and id  = @id

	fetch next from c into @account_id, @id
end
CLOSE c;  
DEALLOCATE c;  

drop function dbo.get_date
go
drop function dbo.get_first_date
go