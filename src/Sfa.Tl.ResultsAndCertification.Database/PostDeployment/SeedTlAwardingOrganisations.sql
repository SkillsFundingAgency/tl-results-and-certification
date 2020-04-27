/*
Insert initial data for Tl Awarding Organisations
*/

SET IDENTITY_INSERT [dbo].[TlAwardingOrganisation] ON

MERGE INTO [dbo].[TlAwardingOrganisation] AS Target 
USING (VALUES 
  (1, 10009696, N'Ncfe', N'Ncfe'),
  (2, 10022490, N'Pearson', N'Pearson'),
  (3, 10009931, N'City & Guilds', N'City & Guilds')
  )
  AS Source ([Id], [UkPrn], [Name], [DisplayName]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[UkPrn] <> Source.[UkPrn])
	   OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	   OR (Target.[DisplayName] <> Source.[DisplayName] COLLATE Latin1_General_CS_AS))
THEN 
UPDATE SET 
	[UkPrn] = Source.[UkPrn],
	[Name] = Source.[Name],
	[DisplayName] = Source.[DisplayName],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [UkPrn], [Name], [DisplayName], [CreatedBy]) 
	VALUES ([Id], [UkPrn], [Name], [DisplayName], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlAwardingOrganisation] OFF
