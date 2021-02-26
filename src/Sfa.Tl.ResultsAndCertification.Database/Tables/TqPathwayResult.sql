CREATE TABLE [dbo].[TqPathwayResult]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[TqPathwayAssessmentId] INT NOT NULL,
	[TlLookupId] INT NOT NULL,
	[StartDate] DATETIME NOT NULL,	
	[EndDate] DATETIME NULL,
	[IsOptedin] BIT NOT NULL DEFAULT 1,
	[IsBulkUpload] BIT NOT NULL DEFAULT 0,	
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqPathwayResult] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqPathwayResult_TqPathwayAssessment] FOREIGN KEY ([TqPathwayAssessmentId]) REFERENCES [TqPathwayAssessment]([Id]),
	CONSTRAINT [FK_TqPathwayResult_TlLookup] FOREIGN KEY ([TlLookupId]) REFERENCES [TlLookup]([Id]),
	INDEX IX_TqPathwayResult_TqPathwayAssessmentId_TlLookupId NONCLUSTERED (TqPathwayAssessmentId, TlLookupId)
)