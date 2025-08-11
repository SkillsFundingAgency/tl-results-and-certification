/*
Insert initial data for TlDualSpecialismToSpecialism
*/
SET IDENTITY_INSERT [dbo].[TlDualSpecialismToSpecialism] ON

MERGE INTO [dbo].[TlDualSpecialismToSpecialism] AS Target 
USING (VALUES 
  (1,1,11),
  (2,1,13),
  (3,2,11),  
  (4,2,16), 
  (5,3,14), 
  (6,3,15),
  (7,4,83),
  (8,4,84)
  )
  AS Source ([Id], [TlDualSpecialismId], [TlSpecialismId]) 
ON Target.[TlDualSpecialismId] = Source.[TlDualSpecialismId] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Id] <> Source.[Id])	
	 AND (Target.[TlDualSpecialismId] <> Source.[TlDualSpecialismId])	
	 AND (Target.[TlSpecialismId] <> Source.[TlSpecialismId])	
	 )
THEN 
UPDATE SET 	
	[TlDualSpecialismId] = Source.[TlDualSpecialismId],
	[TlSpecialismId] = Source.[TlSpecialismId]	
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [TlDualSpecialismId], [TlSpecialismId]) 
	VALUES ([Id], [TlDualSpecialismId], [TlSpecialismId]) 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlDualSpecialismToSpecialism] OFF
