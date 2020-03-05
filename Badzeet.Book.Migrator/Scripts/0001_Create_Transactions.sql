create table [book].[transactions](
	[id] [bigint] identity(1,1) primary key,
	[account_id] [bigint] not null,
	[date] [date] not null,
	[amount] [decimal](10, 2) not null,
	[description] [nvarchar](100) null
)