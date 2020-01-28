CREATE TABLE [dbo].[UniversityData]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [CountryID] INT NOT NULL REFERENCES [dbo].[CountryData](ID)
)
