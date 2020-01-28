using System;
using System.Collections.Generic;

namespace MSPApp.DB
{
    public partial class UniversityData
    {
        public UniversityData()
        {
            UserData = new HashSet<UserData>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }

        public virtual CountryData Country { get; set; }
        public virtual ICollection<UserData> UserData { get; set; }
    }
}
