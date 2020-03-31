CREATE TABLE [dbo].[NotificationTemplate]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TemplateId] UNIQUEIDENTIFIER NOT NULL,
	[TemplateName] NVARCHAR(50) NOT NULL, 	 
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[CreatedBy] NVARCHAR(50) NULL, 
	[ModifiedOn] DATETIME2 NULL, 
	[ModifiedBy] NVARCHAR(50) NULL,
    CONSTRAINT [PK_NotificationTemplates] PRIMARY KEY ([Id])
)
