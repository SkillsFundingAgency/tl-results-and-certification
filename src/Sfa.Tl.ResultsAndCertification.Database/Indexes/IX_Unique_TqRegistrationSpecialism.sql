CREATE UNIQUE NONCLUSTERED INDEX [IX_Unique_TqRegistrationSpecialism] ON TqRegistrationSpecialism 
(
    TqRegistrationPathwayId, TlSpecialismId
)
WHERE ([Status] = 1)