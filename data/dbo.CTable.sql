CREATE TABLE [dbo].[CTable] (
    [CGrade] TEXT   NOT NULL,
    [eb2]    REAL NULL DEFAULT 0,
    [eb0]    REAL NULL DEFAULT 0,
    [ebt0]   REAL NULL DEFAULT 0,
    [ebt2]   REAL NULL DEFAULT 0,
    [Rb]     REAL NULL DEFAULT 0,
    [Rbt]    REAL NULL DEFAULT 0,
    [Eb]     REAL NULL DEFAULT 0,
    CONSTRAINT [PK_CTable] PRIMARY KEY CLUSTERED ([CGrade] ASC)
);

