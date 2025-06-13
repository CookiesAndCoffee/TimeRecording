CREATE TABLE [dbo].[Arbeitszeit] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [PersonenId] INT NOT NULL,
    [Datum] DATE NOT NULL,
    [Minuten] SMALLINT NOT NULL,
    CONSTRAINT [FK_Arbeitszeit_Personen] FOREIGN KEY ([PersonenId]) REFERENCES [dbo].[Personen]([Id])
);