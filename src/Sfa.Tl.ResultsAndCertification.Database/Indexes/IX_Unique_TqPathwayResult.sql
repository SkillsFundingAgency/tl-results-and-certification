CREATE UNIQUE NONCLUSTERED INDEX [IX_Unique_TqPathwayResult] ON TqPathwayResult
(
    TqPathwayAssessmentId
)
WHERE ([IsOptedin] = 1 AND [EndDate] IS NULL)