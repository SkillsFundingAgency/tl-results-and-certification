CREATE TABLE [dbo].[TqSpecialismAssessment]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[TqRegistrationSpecialismId] INT NOT NULL,
	[AssessmentSeriesId] INT NOT NULL,
	[StartDate] DATETIME NOT NULL,	
	[EndDate] DATETIME NULL,
	[IsOptedin] BIT NOT NULL DEFAULT 1,
	[IsBulkUpload] BIT NOT NULL DEFAULT 0,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqSpecialismAssessment] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqSpecialismAssessment_TqRegistrationSpecialism] FOREIGN KEY ([TqRegistrationSpecialismId]) REFERENCES [TqRegistrationSpecialism]([Id]),
	CONSTRAINT [FK_TqSpecialismAssessment_AssessmentSeries] FOREIGN KEY ([AssessmentSeriesId]) REFERENCES [AssessmentSeries]([Id]),
	INDEX IX_TqSpecialismAssessment_TqRegistrationSpecialismId NONCLUSTERED (TqRegistrationSpecialismId)
)