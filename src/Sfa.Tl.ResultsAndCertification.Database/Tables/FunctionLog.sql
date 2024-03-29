﻿CREATE TABLE [dbo].[FunctionLog]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[FunctionType] INT NOT NULL DEFAULT 0,
	[Name] NVARCHAR(255) NOT NULL,
	[StartDate] DATETIME NOT NULL,	
	[EndDate] DATETIME NULL,
	[Status] INT NOT NULL,
	[Message] NVARCHAR(MAX) NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL,
    [ModifiedOn] DATETIME2 NULL,
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_FunctionLog] PRIMARY KEY ([Id])	
)