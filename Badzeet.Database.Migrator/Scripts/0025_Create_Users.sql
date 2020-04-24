create table users.users(
id uniqueidentifier primary key,
username nvarchar(100) not null unique,
password nvarchar(200)
)