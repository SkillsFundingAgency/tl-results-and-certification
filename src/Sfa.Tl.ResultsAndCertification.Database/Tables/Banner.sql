CREATE TABLE [dbo].[Banner] (
	[Id] [int] IDENTITY(1, 1) NOT NULL
	,[Title] [nvarchar](255) NOT NULL
	,[Content] [nvarchar](500) NOT NULL
	,[Target] [int] NOT NULL
	,[Start] DATETIME NOT NULL
	,[End] DATETIME NOT NULL
	,[IsOptedin] BIT NOT NULL DEFAULT 1
	,[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate()
	,[CreatedBy] [nvarchar](50) NULL
	,[ModifiedOn] [datetime2](7) NULL
	,[ModifiedBy] [nvarchar](50) NULL
	,CONSTRAINT [PK_Banner] PRIMARY KEY ([Id])
	,CONSTRAINT [CHK_Target] CHECK ([Target] IN (1, 2, 3))
	,CONSTRAINT [CHK_Start_End] CHECK ([End] > [Start])
	)