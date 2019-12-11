CREATE TABLE [dbo].[TlMandatoryAdditionalRequirement]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[IsRegulatedQualification] BIT NOT NULL DEFAULT 0,
	[LarId] NVARCHAR(8) NULL,     
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlMandatoryAdditionalRequirement] PRIMARY KEY ([Id]),
	CONSTRAINT if_isregulatedqualification_set_to_true_then_larid_is_required
	CHECK ( NOT (IsRegulatedQualification = 1 AND LarId IS NULL) )
	--CHECK ( (NOT IsRegulatedQualification = 1) OR (LarId IS NOT NULL) ) 
)
