CREATE TABLE [dbo].[ActivityDetails]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [ActivityID] INT NOT NULL REFERENCES [dbo].[ActivityType](ID),
    [DataType] NVARCHAR (15) NOT NULL DEFAULT('STRING'),
	[Name] NVARCHAR(MAX) NOT NULL
)
