CREATE TABLE [dbo].[Notification] (
	[Id] [int] IDENTITY(1, 1) NOT NULL
	,[Title] [nvarchar](255) NOT NULL
	,[Content] [nvarchar](500) NOT NULL
	,[Target] [int] NOT NULL
	,[Start] DATE NOT NULL
	,[End] DATE NOT NULL
	,[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate()
	,[CreatedBy] [nvarchar](50) NULL
	,[ModifiedOn] [datetime2](7) NULL
	,[ModifiedBy] [nvarchar](50) NULL
	,CONSTRAINT [PK_Notification] PRIMARY KEY ([Id])
	,CONSTRAINT [CHK_Target] CHECK ([Target] IN (1, 2, 3))
	,CONSTRAINT [CHK_Start_End] CHECK ([End] >= [Start])
	)