-- TLRC - 6122 - Maths and English split
-- Need to delete this file after this release - Release - 19

UPDATE [dbo].[TqRegistrationProfile] SET [MathsStatus] = CASE WHEN IsEnglishAndMathsAchieved = 1 THEN 1 ELSE 2 END, [EnglishStatus] = CASE WHEN IsEnglishAndMathsAchieved = 1 THEN 1 ELSE 2 END WHERE [IsRcFeed] = 1 AND [IsEnglishAndMathsAchieved] IS NOT NULL;

TRUNCATE TABLE [dbo].[QualificationAchieved];