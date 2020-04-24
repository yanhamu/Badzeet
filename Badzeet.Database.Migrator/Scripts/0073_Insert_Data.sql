insert into budget.users(id, nickname)
select distinct user_id, nickname
from book.users u
join book.user_books ub
	on ub.user_id = u.id

set identity_insert budget.accounts on
insert into budget.accounts(id, first_day_of_budget, created_at)
select id, first_day_of_budget, created from book.books
set identity_insert budget.accounts off

insert into budget.user_accounts([user_id], account_id)
select [user_id], book_id from book.user_books

set identity_insert budget.categories on 
insert into budget.categories(id, account_id, [name], [order])
select id, book_id, [name], [order] from book.categories
set identity_insert budget.categories off

set identity_insert budget.transactions on
insert into budget.transactions(id, account_id, [date], amount, [description], category_id, owner_id)
select id, book_id, [date], amount, [description], category_id, owner_id from book.transactions
set identity_insert budget.transactions off
