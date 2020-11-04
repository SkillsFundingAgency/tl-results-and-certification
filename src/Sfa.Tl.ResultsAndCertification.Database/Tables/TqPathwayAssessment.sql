CREATE TABLE [dbo].[TqPathwayAssessment]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[TqRegistrationPathwayId] INT NOT NULL,
	[AssessmentSeriesId] INT NOT NULL,
	[StartDate] DATETIME NOT NULL,	
	[EndDate] DATETIME NULL,
	[IsOptedin] BIT NOT NULL DEFAULT 1,
	[IsBulkUpload] BIT NOT NULL DEFAULT 0,	
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqPathwayAssessment] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqPathwayAssessment_TqRegistrationPathway] FOREIGN KEY ([TqRegistrationPathwayId]) REFERENCES [TqRegistrationPathway]([Id]),
	CONSTRAINT [FK_TqPathwayAssessment_AssessmentSeries] FOREIGN KEY ([AssessmentSeriesId]) REFERENCES [AssessmentSeries]([Id]),
	INDEX IX_TqPathwayAssessment_TqRegistrationPathwayId NONCLUSTERED (TqRegistrationPathwayId)
)