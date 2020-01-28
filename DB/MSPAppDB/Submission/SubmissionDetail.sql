CREATE TABLE [dbo].[SubmissionDetail]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [SubmissionID] INT NOT NULL REFERENCES [dbo].[Submission](ID), 
    [ActivityDetailID] INT NOT NULL REFERENCES [dbo].[ActivityDetails](ID),
    [Value] NVARCHAR(MAX) NULL
)
