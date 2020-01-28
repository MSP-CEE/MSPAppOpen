namespace MSPApp.Infrastructure
{
    public static class Constants
    {
        public const string MailKey = "UserPrincipalName";
        public const string CountryKey = "Country";
        public const string UniversityKey = "Department";
        public const string NameKey = "DisplayName";

        public const string ExistingActivity = "NOSpam";
        public const string NewActivity = "MayBeSpam";

        public const string MSPsKey = "MSPMailsData";
        public const string StartingIndexKey = "StartIndex";
        public const string SubmissionKey = "SubmissionData";
        public const string PictureKey = "ImageData";
        public const string UserKey = "UserData";
        public const string ActivityKey = "ActivityData";
        public const string ActivityDictionaryKey = "ActivityDictionaryData";
        public const string PreviousActivitiesKey = "PreviousActivitiesData";

        public const string NoData = "Missing Data";
        public const string ScopeUserRead = "User.Read";
        public const string BearerAuthorizationScheme = "Bearer";
        public const string DBCS = "Data Source=;Initial Catalog=MSPApp;User ID=;Password=;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    }
}