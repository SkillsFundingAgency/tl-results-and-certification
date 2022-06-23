CREATE TABLE [dbo].[OverallResult]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[TqRegistrationPathwayId] INT NOT NULL,
	[Details] NVARCHAR(MAX) NULL,
	[ResultAwarded] NVARCHAR(50),
	[CalculationStatus] INT NOT NULL,
	[PublishDate] DATETIME NULL,
	[PrintAvailableFrom] DATETIME NULL,
	[StartDate] DATETIME NOT NULL,	
	[EndDate] DATETIME NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_OverallResult] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_OverallResult_TqRegistrationPathway] FOREIGN KEY ([TqRegistrationPathwayId]) REFERENCES [TqRegistrationPathway]([Id])
)