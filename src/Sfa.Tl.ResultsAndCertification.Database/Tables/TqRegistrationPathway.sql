CREATE TABLE [dbo].[TqRegistrationPathway]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[TqRegistrationProfileId] INT NOT NULL,
	[TqProviderId] INT NOT NULL,
	[AcademicYear] INT NOT NULL,
	[StartDate] DATETIME NOT NULL,	
	[EndDate] DATETIME NULL,
	[Status] INT NOT NULL,
	[IsBulkUpload] BIT NOT NULL DEFAULT 0,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqRegistrationPathway] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqRegistrationPathway_TqRegistrationProfile] FOREIGN KEY ([TqRegistrationProfileId]) REFERENCES [TqRegistrationProfile]([Id]),
	CONSTRAINT [FK_TqRegistrationPathway_TqProvider] FOREIGN KEY ([TqProviderId]) REFERENCES [TqProvider]([Id]),
	INDEX IX_TqRegistrationPathway_RegistrationProfileId NONCLUSTERED (TqRegistrationProfileId)
)