using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using MSPApp.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MSPApp.Infrastructure
{
    public static class Utils
    {
        static readonly string[] propertiesToShow = new string[] {
                Constants.CountryKey,
                Constants.UniversityKey,
                Constants.NameKey,
                Constants.MailKey };

        /// <summary>
        /// Takes only some properties from the JSON
        /// </summary>
        /// <param name="givenService"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDataFromGraphUser(this User givenUser)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            PropertyInfo[] properties = givenUser.GetType().GetProperties();

            foreach (PropertyInfo currentProperty in properties)
            {
                string propertyName = currentProperty.Name;
                if (!propertiesToShow.Contains(propertyName))
                    continue;

                string propertyValue = currentProperty.GetValue(givenUser)?.ToString();
                data[propertyName] = propertyValue ?? Constants.NoData;
            }

            return data;
        }

        /// <summary>
        /// Gets the data from the Graph object and in case it does not exist in the
        /// database it is addded.
        /// </summary>
        /// <param name="givenUser"></param>
        /// <returns>UserData object pointing to the Database</returns>
        public static async Task<UserData> ToDBObject(this User givenUser)
        {
            Dictionary<string, string> data = givenUser.GetDataFromGraphUser();

            using MSPAppContext dbContext = new MSPAppContext();
            string mspMail = data[Constants.MailKey];
            string countryName = data[Constants.CountryKey];
            string universityName = data[Constants.UniversityKey];

            if (!dbContext.UserData.Any(x => x.Mspmail == mspMail))
            {
                if (!dbContext.CountryData.Any(x => x.Name == countryName))
                {
                    await dbContext.CountryData.AddAsync(
                        new CountryData
                        {
                            Name = countryName,
                            Id = dbContext.CountryData.Count()
                        });
                    dbContext.SaveChanges();
                }
                CountryData currentCountry =
                    dbContext.CountryData.FirstOrDefault(x => x.Name == countryName);

                if (!dbContext.UniversityData.Any(x => x.Name == universityName))
                {
                    await dbContext.UniversityData.AddAsync(
                        new UniversityData
                        {
                            Name = universityName,
                            Id = dbContext.UniversityData.Count()
                        });

                    dbContext.SaveChanges();
                }
                UniversityData currentUniversity =
                    dbContext.UniversityData.FirstOrDefault(x => x.Name == universityName);

                await dbContext.UserData.AddAsync(
                    new UserData
                    {
                        Mspmail = mspMail,
                        CountryId = currentCountry.Id,
                        UniversityId = currentUniversity.Id,
                        Name = data[Constants.NameKey],
                        Id = dbContext.UserData.Count()
                    });

                await dbContext.SaveChangesAsync();
            }

            return dbContext.UserData.FirstOrDefault(x => x.Mspmail == mspMail);
        }

        public static async Task<(User, string)> GetCurrentUserFromGraph(this GraphServiceClient givenService)
        {
            User graphUser = await givenService.Me.Request().GetAsync();
            string base64Image = null;

            try
            {
                // Get user photo
                using var photoStream = await givenService.Me.Photo.Content.Request().GetAsync();
                byte[] photoByte = ((MemoryStream)photoStream).ToArray();
                base64Image = Convert.ToBase64String(photoByte);
            }
            catch { }

            return (graphUser, base64Image);
        }

        public static T GetFromJSONTo<T>(this ISession givenSession, string key)
        {
            return givenSession.Keys.Contains(key)
                ? JsonConvert.DeserializeObject<T>(givenSession.GetString(key))
                : default;
        }

        public static void AddToJSONFrom<T>(this ISession givenSession, string key, T givenData)
        {
            givenSession.SetString(key, JsonConvert.SerializeObject(givenData));
        }
    }
}
