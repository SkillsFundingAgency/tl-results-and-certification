CREATE UNIQUE NONCLUSTERED INDEX [IX_Unique_TqSpecialismAssessment] ON TqSpecialismAssessment 
(
    TqRegistrationSpecialismId, AssessmentSeriesId
)
WHERE ([IsOptedin] = 1 AND [EndDate] IS NULL)