create table book.books(
	id bigint identity(1,1) primary key,
	first_day_of_budget tinyint not null
)

insert into book.books(first_day_of_budget) values(5)

declare @id bigint = SCOPE_IDENTITY()

update book.transactions set book_id = @id where book_id = 1
