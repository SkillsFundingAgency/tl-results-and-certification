/*
Insert initial data for Tq Awarding Organisations
*/

SET IDENTITY_INSERT [dbo].[TqAwardingOrganisation] ON

MERGE INTO [dbo].[TqAwardingOrganisation] AS Target 
USING (VALUES 
  (1, N'P123', N'Pearson', N'Pearson'),
  (2, N'Ncfe123', N'Ncfe', N'Ncfe')
  )
  AS Source ([Id], [UkAon], [Name], [DisplayName]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[UkAon] <> Source.[UkAon] COLLATE Latin1_General_CS_AS)
	   OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	   OR (Target.[DisplayName] <> Source.[DisplayName] COLLATE Latin1_General_CS_AS))
THEN 
UPDATE SET 
	[UkAon] = Source.[UkAon],
	[Name] = Source.[Name],
	[DisplayName] = Source.[DisplayName],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [UkAon], [Name], [DisplayName], [CreatedBy]) 
	VALUES ([Id], [UkAon], [Name], [DisplayName], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TqAwardingOrganisation] OFF
