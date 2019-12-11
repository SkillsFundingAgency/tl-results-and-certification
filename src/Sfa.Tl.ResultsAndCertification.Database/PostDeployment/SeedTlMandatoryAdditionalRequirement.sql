/*
Insert initial data for TlMandatoryAdditionalRequirement
*/

SET IDENTITY_INSERT [dbo].[TlMandatoryAdditionalRequirement] ON

MERGE INTO [dbo].[TlMandatoryAdditionalRequirement] AS Target 
USING (VALUES 
  (1, N'Surveying and design for construction and the built environment', 1, N'11134567'),
  (2, N'Civil Engineering', 1, N'11234567'),
  (3, N'Building services design', 1, N'11324567'),
  (4, N'Hazardous materials analysis and surveying', 1, N'11423567'),
  (5, N'Early years education and childcare', 1, N'11523467'),
  (6, N'Assisting teaching', 0, NULL),
  (7, N'Supporting and mentoring students in further and higher education', 0, NULL),
  (8, N'Digital Production, Design and Development', 0, NULL)
  )
  AS Source ([Id], [Name], [IsRegulatedQualification], [LarId]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)	 
	 OR (Target.[IsRegulatedQualification] <> Source.[IsRegulatedQualification])
	 OR (Target.[LarId] <> Source.[LarId] COLLATE Latin1_General_CS_AS)) 
THEN 
UPDATE SET 
	[Name] = Source.[Name],
	[IsRegulatedQualification] = source.[IsRegulatedQualification],
	[LarId] = Source.[LarId],	
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Name], [IsRegulatedQualification], [LarId], [CreatedBy]) 
	VALUES ([Id], [Name], [IsRegulatedQualification], [LarId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlMandatoryAdditionalRequirement] OFF
