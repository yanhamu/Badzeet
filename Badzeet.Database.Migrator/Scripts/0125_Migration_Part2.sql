select id, account_id, [date], amount, [description], dbo.get_id(category_id) as category_id, owner_id, [type], [uid]
into budget.p
from budget.payments
go
