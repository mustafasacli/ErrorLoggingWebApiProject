-- Script Date: 25.04.2019 21:28  - ErikEJ.SqlCeScripting version 3.5.2.80
CREATE TABLE [ErrorLog] (
  [Id] nvarchar(50) NOT NULL
, [RequestAddres] nvarchar(1000) NULL
, [ResponseAddress] nvarchar(1000) NULL
, [ResponseMachineName] nvarchar(100) NULL
, [UserId] nvarchar(100) NULL
, [ClassName] nvarchar(200) NULL
, [MethodName] nvarchar(200) NULL
, [Message] nvarchar(4000) NULL
, [StackTrace] nvarchar(4000) NULL
, [ExceptionData] nvarchar(4000) NULL
, [LogTime] datetime NULL
, [LogTimeUnixTimestamp] bigint NULL
, [CreatedOn] datetime NULL
, [CreatedOnUnixTimestamp] bigint NULL
);
GO
ALTER TABLE [ErrorLog] ADD CONSTRAINT [PK_ErrorLog] PRIMARY KEY ([Id]);
GO