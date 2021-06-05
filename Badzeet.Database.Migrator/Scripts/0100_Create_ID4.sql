create schema [id4]

create table [id4].[DeviceCodes] (
    [UserCode] nvarchar(200) NOT NULL,
    [DeviceCode] nvarchar(200) NOT NULL,
    [SubjectId] nvarchar(200) NULL,
    [SessionId] nvarchar(100) NULL,
    [ClientId] nvarchar(200) NOT NULL,
    [Description] nvarchar(200) NULL,
    [CreationTime] datetime2 NOT NULL,
    [Expiration] datetime2 NOT NULL,
    [Data] nvarchar(max) NOT NULL,
    constraint [PK_DeviceCodes] primary key ([UserCode])
);

go

create table [id4].[PersistedGrants] (
    [Key] nvarchar(200) NOT NULL,
    [Type] nvarchar(50) NOT NULL,
    [SubjectId] nvarchar(200) NULL,
    [SessionId] nvarchar(100) NULL,
    [ClientId] nvarchar(200) NOT NULL,
    [Description] nvarchar(200) NULL,
    [CreationTime] datetime2 NOT NULL,
    [Expiration] datetime2 NULL,
    [ConsumedTime] datetime2 NULL,
    [Data] nvarchar(max) NOT NULL,
    constraint [PK_PersistedGrants] primary key ([Key])
);

go

create unique index [IX_DeviceCodes_DeviceCode] ON [id4].[DeviceCodes] ([DeviceCode]);

go

create index [IX_DeviceCodes_Expiration] ON [id4].[DeviceCodes] ([Expiration]);

go

create index [IX_PersistedGrants_Expiration] ON [id4].[PersistedGrants] ([Expiration]);

go

create index [IX_PersistedGrants_SubjectId_ClientId_Type] ON [id4].[PersistedGrants] ([SubjectId], [ClientId], [Type]);

go

create index [IX_PersistedGrants_SubjectId_SessionId_Type] ON [id4].[PersistedGrants] ([SubjectId], [SessionId], [Type]);

go