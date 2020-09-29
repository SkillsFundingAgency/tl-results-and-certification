CREATE UNIQUE NONCLUSTERED INDEX [IX_Unique_TqRegistrationSpecialism_IsOptedin] ON TqRegistrationSpecialism 
(
    TqRegistrationPathwayId, TlSpecialismId
)
WHERE ([IsOptedin] = 1 AND [EndDate] IS NULL)