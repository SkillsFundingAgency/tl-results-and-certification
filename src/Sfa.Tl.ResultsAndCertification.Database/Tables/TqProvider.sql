﻿CREATE TABLE [dbo].[TqProvider]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
    [TqAwardingOrganisationId] INT NOT NULL,
	[TlProviderId] INT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqProvider] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqProvider_TqAwardingOrganisation] FOREIGN KEY ([TqAwardingOrganisationId]) REFERENCES [TqAwardingOrganisation]([Id]),
	CONSTRAINT [FK_TqProvider_TlProvider] FOREIGN KEY ([TlProviderId]) REFERENCES [TlProvider]([Id])
)
