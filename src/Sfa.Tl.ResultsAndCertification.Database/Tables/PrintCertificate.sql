CREATE TABLE [dbo].[PrintCertificate]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
    [PrintBatchItemId] INT NOT NULL,
    [TqRegistrationPathwayId] INT NOT NULL,
    [CertificateNumber] AS FORMAT([Id], '0000000#'),
    [Uln] BIGINT NOT NULL,
    [LearnerName] NVARCHAR(256) NOT NULL,    
    [Type] INT NOT NULL,
    [LearningDetails] NVARCHAR(MAX) NULL,
    [DisplaySnapshot] NVARCHAR(MAX) NULL,    
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_PrintCertificate] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PrintCertificate_PrintBatchItem] FOREIGN KEY ([PrintBatchItemId]) REFERENCES [PrintBatchItem]([Id]),
    CONSTRAINT [FK_PrintCertificate_TqRegistrationPathway] FOREIGN KEY ([TqRegistrationPathwayId]) REFERENCES [TqRegistrationPathway]([Id]),
	INDEX IX__PrintCertificate_PrintBatchItemId NONCLUSTERED ([PrintBatchItemId]),
    INDEX IX__PrintCertificate_Uln_TqRegistrationPathwayId NONCLUSTERED ([Uln], [TqRegistrationPathwayId])
)