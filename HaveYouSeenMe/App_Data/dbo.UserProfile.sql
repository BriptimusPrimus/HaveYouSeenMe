CREATE TABLE [dbo].[UserProfile] (
    [UserId]    INT            IDENTITY (1, 1) NOT NULL,
    [UserName]  NVARCHAR (MAX) NULL,
    [FirstName] VARCHAR (150)  NULL,
    [LastName]  VARCHAR (150)  NULL,
    [Email]     VARCHAR (150)  NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

