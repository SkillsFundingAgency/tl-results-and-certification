CREATE TABLE [dbo].[TlPathwaySpecialismMar]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[MarId] INT NOT NULL,
	[PathwayId] INT NULL,
	[SpecialismId] INT NULL,	
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlPathwaySpecialismMar] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TlPathwaySpecialismMar_TlPathway] FOREIGN KEY ([PathwayId]) REFERENCES [TlPathway]([Id]),
	CONSTRAINT [FK_TlPathwaySpecialismMar_TlSpecialism] FOREIGN KEY ([SpecialismId]) REFERENCES [TlSpecialism]([Id]),
	CONSTRAINT check_constraint_for_either_pathwayid_or_specialismid_should_be_specified
	CHECK ((PathwayId IS NULL OR SpecialismId IS NULL) AND NOT(PathwayId IS NULL AND SpecialismId IS NULL))
	--CHECK ((NOT(PathwayId IS NOT NULL AND SpecialismId IS NULL)) OR (NOT((PathwayId IS NULL AND SpecialismID IS NOT NULL))))
	--CHECK (NOT((((PathwayId IS NOT NULL) AND (SpecialismId IS NULL))) OR (((PathwayId IS NULL) AND (SpecialismID IS NOT NULL)))))
	--CHECK (NOT((PathwayId IS NOT NULL AND SpecialismId IS NULL) OR (PathwayId IS NULL AND SpecialismID IS NOT NULL)))
)
