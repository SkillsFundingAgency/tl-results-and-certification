CREATE TABLE [dbo].[TqAwardingOrganisation]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TlAwardingOrganisatonId] INT NOT NULL,
	[TlPathwayId] INT NOT NULL,
    [ReviewStatus] INT NOT NULL DEFAULT 1, -- 1 Awaiting Confirmation, 2 - Confirmed, 3 - Queried 
	[IsActive] BIT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqAwardingOrganisation] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqAwardingOrganisation_TlAwardingOrganisation] FOREIGN KEY ([TlAwardingOrganisatonId]) REFERENCES [TlAwardingOrganisation]([Id]),
	CONSTRAINT [FK_TqAwardingOrganisation_TlPathway] FOREIGN KEY ([TlPathwayId]) REFERENCES [TlPathway]([Id])
)
