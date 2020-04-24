insert into book.categories(book_id, name) select id, 'unspecified' from book.books

go

create function get_category_id(@book_id bigint)
returns bigint
begin
declare @id bigint
select top 1 @id = id from book.categories where book_id = @book_id and name = 'unspecified'
return @id
end

go

update book.transactions set category_id = dbo.get_category_id(book_id) where category_id is null
go 

drop function get_category_id