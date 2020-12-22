CREATE UNIQUE NONCLUSTERED INDEX [IX_Unique_TqPathwayAssessment] ON TqPathwayAssessment 
(
    TqRegistrationPathwayId, AssessmentSeriesId
)
WHERE ([IsOptedin] = 1 AND [EndDate] IS NULL)