/*
Insert initial data for TlDualSpecialism
*/

SET IDENTITY_INSERT [dbo].[TlDualSpecialism] ON

MERGE INTO [dbo].[TlDualSpecialism] AS Target 
USING (VALUES 
  (1, 3,'ZTLOS030','Plumbing and Heating Engineering', 1),
  (2, 3,'ZTLOS031','Heating Engineering and Ventilation', 1),
  (3, 3,'ZTLOS032','Refrigeration Engineering and Air Conditioning Engineering', 1)  
  )
  AS Source ([Id], [TlPathwayId], [LarId], [Name], [IsActive]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)	
	 )
THEN 
UPDATE SET 	
	[TlPathwayId] = Source.[TlPathwayId],
	[LarId] = Source.[LarId],
	[Name] = Source.[Name],
	[IsActive] = Source.[IsActive],
	[CreatedOn] = GETDATE(),
	[CreatedBy] = 'System',
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [TlPathwayId], [LarId], [Name], [IsActive], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) 
	VALUES ([Id], [TlPathwayId], [LarId], [Name], 'System', [ModifiedOn], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlDualSpecialism] OFF
