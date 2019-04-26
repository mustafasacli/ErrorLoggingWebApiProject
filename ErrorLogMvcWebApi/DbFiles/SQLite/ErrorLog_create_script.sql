CREATE TABLE ErrorLog (
  Id text NOT NULL PRIMARY KEY
, RequestAddres text NULL
, ResponseAddress text NULL
, ResponseMachineName text NULL
, UserId text NULL
, ClassName text NULL
, MethodName text NULL
, Message text NULL
, StackTrace text NULL
, ExceptionData text NULL
, LogTime datetime NULL
, LogTimeUnixTimestamp number NULL
, CreatedOn datetime NULL
, CreatedOnUnixTimestamp number NULL
);