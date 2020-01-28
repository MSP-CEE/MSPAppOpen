CREATE TABLE [dbo].[Submission]
(
	[ID] INT NOT NULL PRIMARY KEY,
    [UserID] INT NOT NULL REFERENCES [dbo].[UserData](ID), 
    [ActivityID] INT NOT NULL REFERENCES [dbo].[ActivityType](ID),
    [AnythingElse] NVARCHAR(MAX) NULL,
)
