alter table book.categories add [order] int not null constraint default_order default 10;

alter table book.categories
drop constraint default_order;