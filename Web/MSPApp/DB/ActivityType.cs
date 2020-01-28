using System;
using System.Collections.Generic;

namespace MSPApp.DB
{
    public partial class ActivityType
    {
        public ActivityType()
        {
            ActivityDetails = new HashSet<ActivityDetails>();
            Submission = new HashSet<Submission>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ActivityDetails> ActivityDetails { get; set; }
        public virtual ICollection<Submission> Submission { get; set; }
    }
}
