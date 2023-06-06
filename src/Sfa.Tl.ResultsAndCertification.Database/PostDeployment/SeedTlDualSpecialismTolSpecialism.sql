/*
Insert initial data for TlDualSpecialismTolSpecialism
*/


MERGE INTO [dbo].[TlDualSpecialismTolSpecialism] AS Target 
USING (VALUES 
  (1, (select Id from TlSpecialism where LarId='10202102')),
  (1, (select Id from TlSpecialism where LarId='10202101')),
  (2, (select Id from TlSpecialism where LarId='10202101')),  
  (2, (select Id from TlSpecialism where LarId='10202105')), 
  (3, (select Id from TlSpecialism where LarId='10202103')), 
  (3, (select Id from TlSpecialism where LarId='10202104'))
  )
  AS Source ([DualSpecialismId], [SpecialismId]) 
ON Target.[DualSpecialismId] = Source.[DualSpecialismId] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[DualSpecialismId] <> Source.[DualSpecialismId] COLLATE Latin1_General_CS_AS)	
	 )
THEN 
UPDATE SET 	
	[DualSpecialismId] = Source.[DualSpecialismId],
	[SpecialismId] = Source.[SpecialismId]	
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([DualSpecialismId], [SpecialismId]) 
	VALUES ([DualSpecialismId], [SpecialismId]) 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlDualSpecialismTolSpecialism] OFF
