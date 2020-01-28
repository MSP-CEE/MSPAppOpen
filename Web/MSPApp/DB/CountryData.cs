using System;
using System.Collections.Generic;

namespace MSPApp.DB
{
    public partial class CountryData
    {
        public CountryData()
        {
            UniversityData = new HashSet<UniversityData>();
            UserData = new HashSet<UserData>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryLeadId { get; set; }

        public virtual ICollection<UniversityData> UniversityData { get; set; }
        public virtual ICollection<UserData> UserData { get; set; }
    }
}
