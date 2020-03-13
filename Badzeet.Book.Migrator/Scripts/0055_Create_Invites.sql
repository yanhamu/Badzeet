create table book.invitations(
id uniqueidentifier primary key,
book_id bigint foreign key references book.books(id) not null,
owner_id uniqueidentifier foreign key references book.users(id) not null,
used_at datetime2 null,
created_at datetime2 not null
)