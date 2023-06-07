CREATE TABLE [dbo].[TlDualSpecialismToSpecialism]
(
	[DualSpecialismId] INT NOT NULL,
	[SpecialismId] INT NOT NULL,
	CONSTRAINT TlDualSpecialismToSpecialism_UK UNIQUE (DualSpecialismId,SpecialismId),
	CONSTRAINT [FK_TlDualSpecialismToSpecialism_DualSpecialismId] FOREIGN KEY(DualSpecialismId) REFERENCES [TlDualSpecialism]([Id]),
	CONSTRAINT [FK_TlDualSpecialismToSpecialism_SpecialismId] FOREIGN KEY(SpecialismId) REFERENCES [TlSpecialism]([Id])
)
