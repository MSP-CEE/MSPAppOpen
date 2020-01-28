using System;
using System.Collections.Generic;

namespace MSPApp.DB
{
    public partial class UserData
    {
        public UserData()
        {
            Submission = new HashSet<Submission>();
            SubmissionAssociation = new HashSet<SubmissionAssociation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Mspmail { get; set; }
        public int CountryId { get; set; }
        public int UniversityId { get; set; }

        public virtual CountryData Country { get; set; }
        public virtual UniversityData University { get; set; }
        public virtual ICollection<Submission> Submission { get; set; }
        public virtual ICollection<SubmissionAssociation> SubmissionAssociation { get; set; }
    }
}
