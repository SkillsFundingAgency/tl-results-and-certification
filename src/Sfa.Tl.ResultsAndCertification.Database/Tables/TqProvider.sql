CREATE TABLE [dbo].[TqProvider]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
    [AwardingOrganisationId] INT NOT NULL,
	[ProviderId] INT NOT NULL,
	[RouteId] INT NOT NULL,
	[PathwayId] INT NOT NULL,
	[SpecialismId] INT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqProvider] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TqProvider_TqAwardingOrganisation] FOREIGN KEY ([AwardingOrganisationId]) REFERENCES [TqAwardingOrganisation]([Id]),
	CONSTRAINT [FK_TqProvider_TqProvider] FOREIGN KEY ([ProviderId]) REFERENCES [Provider]([Id]),
	CONSTRAINT [FK_TqProvider_TlRoute] FOREIGN KEY ([RouteId]) REFERENCES [TlRoute]([Id]),
	CONSTRAINT [FK_TqProvider_TlPathway] FOREIGN KEY ([PathwayId]) REFERENCES [TlPathway]([Id]),
	CONSTRAINT [FK_TqProvider_TlSpecialism] FOREIGN KEY ([SpecialismId]) REFERENCES [TlSpecialism]([Id])
)
