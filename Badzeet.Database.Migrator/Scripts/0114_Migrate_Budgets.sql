/* for a change I'm adding another identifier for budget */

alter table budget.budgets
add budget_id int null
go;

update budget.budgets set budget_id = cast(format(date, 'yyyyMM') as int)