CREATE TABLE [dbo].[IpTempFlexNavigation]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TlPathwayId] INT NOT NULL,
	[AcademicYear] INT NOT NULL,
	[AskTempFlexibility] BIT NOT NULL,
	[AskBlendedPlacement] BIT NOT NULL,
	[IsActive] BIT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL,
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_IpTempFlexNavigation] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_IpTempFlexNavigation_TlPathway] FOREIGN KEY ([TlPathwayId]) REFERENCES [TlPathway]([Id])	
)