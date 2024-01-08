

CREATE TABLE [dbo].[ChangeLog]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TqRegistrationPathwayId] INT NOT NULL,
	[ChangeType] INT NOT NULL,
	[Details] NVARCHAR(MAX) NOT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[DateOfRequest] DATETIME NOT NULL,
	[ReasonForChange] NVARCHAR(2000) NULL,
	[ZendeskTicketID] NVARCHAR(10) NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NOT NULL,
    [ModifiedOn] DATETIME2 NULL,
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_ChangeLog] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_ChangeLog_TqRegistrationPathway] FOREIGN KEY ([TqRegistrationPathwayId]) REFERENCES [TqRegistrationPathway]([Id]),
)

