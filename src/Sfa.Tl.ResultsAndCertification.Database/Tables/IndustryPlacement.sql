CREATE TABLE [dbo].[IndustryPlacement]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[TqRegistrationPathwayId] INT NOT NULL,
	[Status] INT NOT NULL,
	[Hours] INT NULL,	
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_IndustryPlacement] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_IndustryPlacement_TqRegistrationPathway] FOREIGN KEY ([TqRegistrationPathwayId]) REFERENCES [TqRegistrationPathway]([Id]),
	CONSTRAINT IX_Unique_IndustryPlacement UNIQUE ([TqRegistrationPathwayId])	
)