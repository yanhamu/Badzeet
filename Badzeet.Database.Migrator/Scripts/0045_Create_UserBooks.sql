create table book.user_books(
[user_id] uniqueidentifier not null foreign key references users.users(id),
book_id bigint not null foreign key references book.books(id),
nickname nvarchar(40) not null,
primary key ([user_id], book_id)
)