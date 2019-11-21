CREATE TABLE [user](
    [id] integer NOT NULL PRIMARY KEY,
    [login] text NOT NULL,
    [email] text NULL DEFAULT 'ak@deserea.net',
    [password_hash] text NULL DEFAULT 'nohash',
    [age] int NULL DEFAULT 1488,
    [description] text NULL DEFAULT NULL)

CREATE TABLE [VersionInfo](
    [Version] INTEGER NOT NULL PRIMARY KEY,
    [AppliedOn] DATETIME NULL,
    [Description] TEXT NULL)

CREATE TABLE [currency](
    [id] UNIQUEIDENTIFIER NOT NULL,
    [code] TEXT NOT NULL,
    [name] TEXT NOT NULL,
    [is_available_to_users] INTEGER NOT NULL,
    CONSTRAINT [PK_currency] PRIMARY KEY([id] ASC))

CREATE UNIQUE INDEX [UC_Version] ON [VersionInfo]([Version] ASC)

CREATE UNIQUE INDEX [UX_currency_code] ON [currency]([code] ASC)

