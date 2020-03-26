create function get_date(@bookId bigint) 
returns date 
begin
declare @d date
	select @d = min(date) from book.transactions where book_id = @bookId
	return @d
end
go 

update book.books set book.books.created = dbo.get_date(book.books.id) where books.id = id

drop function dbo.get_date