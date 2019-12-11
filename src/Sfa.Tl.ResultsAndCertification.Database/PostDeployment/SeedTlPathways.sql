/*
Insert initial data for TlPathways
*/

SET IDENTITY_INSERT [dbo].[TlPathway] ON

MERGE INTO [dbo].[TlPathway] AS Target 
USING (VALUES 
  (1, N'10123456', N'Design, Surveying and Planning', 1),
  (2, N'10223456', N'Education', 2),
  (3, N'10323456', N'Digital Production, Design and Development', 3)
  )
  AS Source ([Id], [LarId], [Name], [RouteId]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[LarId] <> Source.[LarId] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[RouteId] <> Source.[RouteId])) 
THEN 
UPDATE SET 
	[LarId] = Source.[LarId],
	[Name] = Source.[Name],
	[RouteId] = Source.[RouteId],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [LarId], [Name], [RouteId], [CreatedBy]) 
	VALUES ([Id], [LarId], [Name], [RouteId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlPathway] OFF
