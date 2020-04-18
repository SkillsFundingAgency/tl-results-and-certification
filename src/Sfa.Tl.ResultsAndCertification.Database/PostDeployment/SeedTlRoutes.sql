/*
Insert initial data for TlRoutes
*/

SET IDENTITY_INSERT [dbo].[TlRoute] ON

MERGE INTO [dbo].[TlRoute] AS Target 
USING (VALUES 
  (1, N'Construction'),
  (2, N'Education and Childcare'),
  (3, N'Digital'),
  (4, N'Health and Science')
  )
  AS Source ([Id], [Name]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
THEN 
UPDATE SET 
	[Name] = Source.[Name],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Name], [CreatedBy]) 
	VALUES ([Id], [Name], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlRoute] OFF
