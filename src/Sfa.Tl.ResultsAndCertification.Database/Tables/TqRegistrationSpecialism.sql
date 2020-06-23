CREATE TABLE [dbo].[TqRegistrationSpecialism]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[TqRegistrationPathwayId] INT NOT NULL,
	[TlSpecialismId] INT NOT NULL,
	[StartDate] DATETIME NOT NULL,
	[EndDate] DATETIME NULL,
	[Status] INT NOT NULL,
	[IsBulkUpload] BIT NOT NULL DEFAULT 0,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqRegistrationSpecialism] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqRegistrationSpecialism_TqRegistrationPathway] FOREIGN KEY ([TqRegistrationPathwayId]) REFERENCES [TqRegistrationPathway]([Id]),
	CONSTRAINT [FK_TqRegistrationSpecialism_TlSpecialism] FOREIGN KEY ([TlSpecialismId]) REFERENCES [TlSpecialism]([Id]),
	INDEX IX_TqRegistrationSpecialism_All NONCLUSTERED (TqRegistrationPathwayId) 
)
