create table users.tokens(
	id uniqueidentifier primary key,
	user_id uniqueidentifier not null foreign key references users.users(id),
	token binary(128) not null,
	expires datetime2 not null
)
go
create index ix_tokens_user_id on users.tokens(user_id)
go 
create index ix_tokens_expires on users.tokens(expires)