CREATE TABLE [dbo].[TqRegistrationProfile]
(
	[Id] INT IDENTITY(1,1) NOT NULL,    
	[UniqueLearnerNumber] BIGINT NOT NULL,
	[Firstname] NVARCHAR(100) NOT NULL, 
	[Lastname] NVARCHAR(100) NOT NULL, 
	[DateofBirth] DATE NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqRegistrationProfile] PRIMARY KEY ([Id]),
	CONSTRAINT Unique_TqRegistrationProfile_Uln UNIQUE ([UniqueLearnerNumber]),
	INDEX IX_TqRegistrationProfile_Uln NONCLUSTERED (UniqueLearnerNumber)
)