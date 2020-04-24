create table book.categories(
	id bigint identity(1,1) primary key,
	book_id bigint not null foreign key references book.books(id),
	name nvarchar(100) not null
)

alter table book.transactions add category_id bigint foreign key references book.categories(id)