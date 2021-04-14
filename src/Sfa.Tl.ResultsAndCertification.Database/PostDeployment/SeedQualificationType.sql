/*
Insert initial data for QualificationType
*/

SET IDENTITY_INSERT [dbo].[QualificationType] ON

MERGE INTO [dbo].[QualificationType] AS Target 
USING (VALUES 
  (1, N'Basic Skills', NULL, 1),
  (2, N'Free Standing Mathematics Qualification', NULL, 1),
  (3, N'Functional Skills', NULL, 1),
  (4, N'Functional Skills (QCF)', NULL, 1),
  (5, N'GCE A Level', NULL, 1),
  (6, N'GCE AS Level', NULL, 1),
  (7, N'GCSE (9 to 1)', NULL, 1),
  (8, N'GCSE (A* to G)', NULL, 1),
  (9, N'GCSE (A* to G)', N'Double Award', 1),
  (10, N'Other General Qualification', N'Level 1/Level 2 Certificate - (A* to G)', 1),
  (11, N'Other General Qualification', N'Level 1/Level 2 Certificate - (9 to 1)', 1),
  (12, N'Other General Qualification', N'Math', 1),
  (13, N'Other General Qualification', N'Pre U Certificate', 1),
  (14, N'Other General Qualification', N'IBO Level1/Level 2 MYP', 1),
  (15, N'Other General Qualification', N'IBO Level 3 Certificate', 1),
  (16, N'Other General Qualification', N'British Sign Language', 1),
  (17, N'Other General Qualification', N'Level 2 Essential Skills Wales', 1),
  (18, N'Other Vocational Qualification', N'British Sign Language', 1),
  (19, N'Project', N'British Sign Language', 1),
  (20, N'QCF', N'British Sign Language', 1),
  (21, N'Vocationally-Related Qualification', N'Level 3 Math', 1),
  (22, N'Vocationally-Related Qualification', N'British Sign Language', 1), 
  (23, N'Functional Skills', N'Entry 3', 1),
  (24, N'Functional Skills (QCF)', N'Entry 3', 1) ,
  (25, N'Functional Skills', N'Level 1', 1),
  (26, N'Functional Skills (QCF)', N'Level 1', 1) 
  )
  AS Source ([Id], [Name], [SubTitle], [IsActive]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[SubTitle] <> Source.[SubTitle] COLLATE Latin1_General_CS_AS)
	 OR (Target.[IsActive] <> Source.[IsActive])	
	 )
THEN 
UPDATE SET 
	[Name] = Source.[Name],
	[SubTitle] = Source.[SubTitle],
	[IsActive] = Source.[IsActive],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Name], [SubTitle], [IsActive], [CreatedBy]) 
	VALUES ([Id], [Name], [SubTitle], [IsActive],'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[QualificationType] OFF
