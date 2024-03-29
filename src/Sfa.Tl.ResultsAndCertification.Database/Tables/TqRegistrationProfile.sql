﻿CREATE TABLE [dbo].[TqRegistrationProfile]
(
	[Id] INT IDENTITY(1,1) NOT NULL,    
	[UniqueLearnerNumber] BIGINT NOT NULL,
	[Firstname] NVARCHAR(100) NOT NULL, 
	[Lastname] NVARCHAR(100) NOT NULL, 
	[DateofBirth] DATE NOT NULL,
	[Gender] NVARCHAR(25) NULL,
	[IsLearnerVerified] BIT NULL,
	[IsEnglishAndMathsAchieved] BIT NULL,
	[IsSendLearner] BIT NULL,
	[IsRcFeed] BIT NULL,
	[MathsStatus] INT NULL,
	[EnglishStatus] INT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqRegistrationProfile] PRIMARY KEY ([Id]),
	CONSTRAINT Unique_TqRegistrationProfile_Uln UNIQUE ([UniqueLearnerNumber])
)