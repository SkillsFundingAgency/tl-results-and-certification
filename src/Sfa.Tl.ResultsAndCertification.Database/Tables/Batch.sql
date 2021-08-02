CREATE TABLE [dbo].[Batch]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
    [Type] INT NOT NULL,
    [Status] INT NOT NULL,
    [Errors] NVARCHAR(MAX) NULL,
    [PrintingStatus] INT NULL,
    [RunOn] DATETIME2 NULL,
    [StatusChangedOn] DATETIME2 NULL,
    [ResponseStatus] INT NULL,
    [ResponseMessage] NVARCHAR(MAX) NULL,
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_Batch] PRIMARY KEY ([Id])
)