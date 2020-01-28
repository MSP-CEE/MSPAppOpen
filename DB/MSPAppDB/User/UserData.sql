CREATE TABLE [dbo].[UserData]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(255) NOT NULL, 
    [MSPMail] NVARCHAR(255) NOT NULL, 
    [CountryID] INT NOT NULL REFERENCES [dbo].[CountryData](ID), 
    [UniversityID] INT NOT NULL REFERENCES [dbo].[UniversityData](ID)
)
