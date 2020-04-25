create table scheduler.logs (
	id bigint identity(1,1) primary key,
	started_at datetime2(0) not null,
	finished_at datetime2(0) not null,
	processed_rows int not null
)