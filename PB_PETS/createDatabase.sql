
CREATE TABLE [dbo].[Usuario] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [nome]        VARCHAR (20)  NOT NULL,
    [sobrenome]   VARCHAR (40)  NOT NULL,
    [email]       VARCHAR (30)  NOT NULL,
    [telefone]    VARCHAR (15)  NULL,
    [endereco]    VARCHAR (40)  NULL,
    [senha]       VARCHAR (100) NOT NULL,
    [dataCriacao] DATETIME      NOT NULL,
    [foto]        VARCHAR (150) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Amizade] (
    [Id]                  INT      IDENTITY (1, 1) NOT NULL,
    [idUsuarioOrigem]     INT      NOT NULL,
    [idUsuarioDestino]    INT      NOT NULL,
    [statusDaSolicitacao] INT      DEFAULT ((0)) NOT NULL,
    [dataCriacao]         DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([idUsuarioOrigem]) REFERENCES [dbo].[Usuario] ([Id]),
    FOREIGN KEY ([idUsuarioDestino]) REFERENCES [dbo].[Usuario] ([Id])
);


CREATE TABLE [dbo].[Publicacao] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [idUsuario]   INT           NULL,
    [texto]       VARCHAR (300) NULL,
    [foto]        VARCHAR (120) NULL,
    [dataCriacao] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([idUsuario]) REFERENCES [dbo].[Usuario] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Curtidas] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [idUsuario]    INT      NOT NULL,
    [idPublicacao] INT      NOT NULL,
    [dataCriacao]  DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([idUsuario]) REFERENCES [dbo].[Usuario] ([Id])
);


CREATE TABLE [dbo].[Comentario] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [idUsuario]    INT           NOT NULL,
    [idPublicacao] INT           NOT NULL,
    [texto]        VARCHAR (200) NOT NULL,
    [dataCriacao]  DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([idUsuario]) REFERENCES [dbo].[Usuario] ([Id]) ON DELETE CASCADE
);

