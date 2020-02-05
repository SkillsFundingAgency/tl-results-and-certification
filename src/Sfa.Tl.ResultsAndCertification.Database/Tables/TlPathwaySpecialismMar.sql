CREATE TABLE [dbo].[TlPathwaySpecialismMar]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TlMandatoryAdditionalRequirementId] INT NOT NULL,
	[TlPathwayId] INT NULL,
	[TlSpecialismId] INT NULL,	
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlPathwaySpecialismMar] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TlPathwaySpecialismMar_TlMandatoryAdditionalRequirement] FOREIGN KEY ([TlMandatoryAdditionalRequirementId]) REFERENCES [TlMandatoryAdditionalRequirement]([Id]),
	CONSTRAINT [FK_TlPathwaySpecialismMar_TlPathway] FOREIGN KEY ([TlPathwayId]) REFERENCES [TlPathway]([Id]),
	CONSTRAINT [FK_TlPathwaySpecialismMar_TlSpecialism] FOREIGN KEY ([TlSpecialismId]) REFERENCES [TlSpecialism]([Id]),
	CONSTRAINT check_constraint_for_either_pathwayid_or_specialismid_should_be_specified
	CHECK ((TlPathwayId IS NULL OR TlSpecialismId IS NULL) AND NOT(TlPathwayId IS NULL AND TlSpecialismId IS NULL))
)
