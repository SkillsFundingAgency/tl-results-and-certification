CREATE TABLE [dbo].[TqSpecialismResult]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[TqSpecialismAssessmentId] INT NOT NULL,
	[TlLookupId] INT NOT NULL,
	[StartDate] DATETIME NOT NULL,	
	[EndDate] DATETIME NULL,
	[PrsStatus] INT NULL,
	[IsOptedin] BIT NOT NULL DEFAULT 1,
	[IsBulkUpload] BIT NOT NULL DEFAULT 0,	
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqSpecialismResult] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqSpecialismResult_TqSpecialismAssessment] FOREIGN KEY ([TqSpecialismAssessmentId]) REFERENCES [TqSpecialismAssessment]([Id]),
	CONSTRAINT [FK_TqSpecialismyResult_TlLookup] FOREIGN KEY ([TlLookupId]) REFERENCES [TlLookup]([Id]),
	INDEX IX_TqSpecialismResult_TqSpecialismAssessmentId_TlLookupId NONCLUSTERED (TqSpecialismAssessmentId, TlLookupId)
)