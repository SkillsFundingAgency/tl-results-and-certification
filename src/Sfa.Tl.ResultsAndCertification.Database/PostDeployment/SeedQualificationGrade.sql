/*
Insert initial data for QualificationGrade
*/

SET IDENTITY_INSERT [dbo].[QualificationGrade] ON

MERGE INTO [dbo].[QualificationGrade] AS Target 
USING (VALUES 
   /* Basic Skills */
  (1, 1, N'Pass', 1, 1),
  (2, 1, N'Fail', 0, 1),

  /* Free Standing Mathematics Qualification */
  (3, 2, N'A', 1, 1),
  (4, 2, N'B', 1, 1),
  (5, 2, N'C', 1, 1),
  (6, 2, N'D', 0, 1),
  (7, 2, N'E', 0, 1),

  /* Functional Skills */
  (8, 3, N'Pass', 1, 1),
  (9, 3, N'Fail', 0, 1),

  /* Functional Skills (QCF) */
  (10, 4, N'Pass', 1, 1),
  (11, 4, N'Fail', 0, 1),

  /* GCE A Level */
  (12, 5, N'A*', 1, 1),
  (13, 5, N'A', 1, 1),
  (14, 5, N'B', 1, 1),
  (15, 5, N'C', 1, 1),
  (16, 5, N'D', 1, 1),
  (17, 5, N'E', 1, 1),
  (18, 5, N'G', 0, 1),

  /* GCE AS Level */
  (19, 6, N'A', 1, 1),
  (20, 6, N'B', 1, 1),
  (21, 6, N'C', 1, 1),
  (22, 6, N'D', 1, 1),
  (23, 6, N'E', 1, 1),

  /* GCSE (9 to 1) */
  (24, 7, N'9', 1, 1),
  (25, 7, N'8', 1, 1),
  (26, 7, N'7', 1, 1),
  (27, 7, N'6', 1, 1),
  (28, 7, N'5', 1, 1),
  (29, 7, N'4', 1, 1),
  (30, 7, N'3', 0, 1),
  (31, 7, N'2', 0, 1),
  (32, 7, N'1', 0, 1),

  /* GCSE (A* to G) */
  (33, 8, N'A*', 1, 1),
  (34, 8, N'A', 1, 1),
  (35, 8, N'B', 1, 1),
  (36, 8, N'C', 1, 1),
  (37, 8, N'D', 0, 1),
  (38, 8, N'E', 0, 1),
  (39, 8, N'F', 0, 1),
  (40, 8, N'G', 0, 1),

  /* GCSE (A* to G) - Double Award */
  (41, 9, N'A*A*', 1, 1),
  (42, 9, N'A*A', 1, 1),
  (43, 9, N'AA', 1, 1),
  (44, 9, N'AB', 1, 1),
  (45, 9, N'BB', 1, 1),
  (46, 9, N'BC', 1, 1),
  (47, 9, N'CC', 1, 1),
  (48, 9, N'CD', 0, 1),
  (49, 9, N'DD', 0, 1),
  (50, 9, N'DE', 0, 1),
  (51, 9, N'EE', 0, 1),
  (52, 9, N'EF', 0, 1),
  (53, 9, N'FF', 0, 1),
  (54, 9, N'FG', 0, 1),
  (55, 9, N'GG', 0, 1),
  
  /* Other General Qualification - Level 1/Level 2 Certificate - (A* to G) */
  (56, 10, N'A*', 1, 1),
  (57, 10, N'A', 1, 1),
  (58, 10, N'B', 1, 1),
  (59, 10, N'C', 1, 1),
  (60, 10, N'D', 0, 1),
  (61, 10, N'E', 0, 1),
  (62, 10, N'F', 0, 1),
  (63, 10, N'G', 0, 1),

    /* Other General Qualification - Level 1/Level 2 Certificate - (9 to 1) */
  (64, 11, N'9', 1, 1),
  (65, 11, N'8', 1, 1),
  (66, 11, N'7', 1, 1),
  (67, 11, N'6', 1, 1),
  (68, 11, N'5', 1, 1),
  (69, 11, N'4', 1, 1),
  (70, 11, N'3', 0, 1),
  (71, 11, N'2', 0, 1),
  (72, 11, N'1', 0, 1),

  /* Other General Qualification - Math */
  (73, 12, N'A', 1, 1),
  (74, 12, N'B', 1, 1),
  (75, 12, N'C', 1, 1),
  (76, 12, N'D', 1, 1),
  (77, 12, N'E', 1, 1),

  /* Other General Qualification - Pre U Certificate */
  (78, 13, N'D1', 1, 1),
  (79, 13, N'D2', 1, 1),
  (80, 13, N'D3', 1, 1),
  (81, 13, N'M1', 1, 1),
  (82, 13, N'M2', 1, 1),
  (83, 13, N'M3', 1, 1),
  (84, 13, N'P1', 1, 1),
  (85, 13, N'P2', 1, 1),
  (86, 13, N'P3', 1, 1),

  /* Other General Qualification - IBO Level1/Level 2 MYP */
  (87, 14, N'7', 1, 1),
  (88, 14, N'6', 1, 1),
  (89, 14, N'5', 1, 1),
  (90, 14, N'4', 1, 1),
  (91, 14, N'3', 1, 1),
  (92, 14, N'2', 0, 1),
  (93, 14, N'1', 0, 1),

  /* Other General Qualification - IBO Level 3 Certificate */
  (94, 15, N'7', 1, 1),
  (95, 15, N'6', 1, 1),
  (96, 15, N'5', 1, 1),
  (97, 15, N'4', 1, 1),
  (98, 15, N'3', 1, 1),
  (99, 15, N'2', 0, 1),
  (100, 15, N'1', 0, 1),

  /* Other General Qualification - British Sign Language */
  (101, 16, N'Pass', 1, 1),
  (102, 16, N'Fail', 0, 1),

  /* Other General Qualification - Level 2 Essential Skills Wales */
  (103, 17, N'Pass', 1, 1),
  (104, 17, N'Fail', 0, 1),

  /* Other Vocational Qualification - British Sign Language */
  (105, 18, N'Pass', 1, 1),
  (106, 18, N'Fail', 0, 1),

  /* Project - British Sign Language */
  (107, 19, N'Pass', 1, 1),
  (108, 19, N'Fail', 0, 1),

  /* QCF - British Sign Language */
  (109, 20, N'Pass', 1, 1),
  (110, 20, N'Fail', 0, 1),

  /* Vocationally-Related Qualification - Level 3 Math */
  (111, 21, N'A', 1, 1),
  (112, 21, N'B', 1, 1),
  (113, 21, N'C', 1, 1),
  (114, 21, N'D', 1, 1),
  (115, 21, N'E', 1, 1),

  /* Vocationally-Related Qualification - British Sign Language */
  (116, 22, N'Pass', 1, 1),
  (117, 22, N'Fail', 0, 1),

  /* Functional Skills - Entry 3 */
  (118, 23, N'Pass', 1, 1),
  (119, 23, N'Fail', 0, 1),

  /* Functional Skills (QCF) - Entry 3 */
  (120, 24, N'Pass', 1, 1),
  (121, 24, N'Fail', 0, 1)
  )
  AS Source ([Id], [QualificationTypeId], [Grade], [IsAllowable], [IsActive]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[QualificationTypeId] <> Source.[QualificationTypeId])     
	 OR (Target.[Grade] <> Source.[Grade] COLLATE Latin1_General_CS_AS)
     OR (Target.[IsAllowable] <> Source.[IsAllowable])
	 OR (Target.[IsActive] <> Source.[IsActive])	
	 )
THEN 
UPDATE SET 	
    [QualificationTypeId] = Source.[QualificationTypeId],
	[Grade] = Source.[Grade],
    [IsAllowable] = Source.[IsAllowable],
	[IsActive] = Source.[IsActive],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [QualificationTypeId], [Grade], [IsAllowable], [IsActive], [CreatedBy]) 
	VALUES ([Id], [QualificationTypeId], [Grade], [IsAllowable], [IsActive],'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[QualificationGrade] OFF
