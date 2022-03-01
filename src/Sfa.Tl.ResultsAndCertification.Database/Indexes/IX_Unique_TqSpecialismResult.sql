CREATE UNIQUE NONCLUSTERED INDEX [IX_Unique_TqSpecialismResult] ON TqSpecialismResult
(
    TqSpecialismAssessmentId
)
WHERE ([IsOptedin] = 1 AND [EndDate] IS NULL)