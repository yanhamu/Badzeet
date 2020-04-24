alter table book.books add [created] date not null constraint default_date default GETDATE();

alter table book.books
drop constraint default_date;