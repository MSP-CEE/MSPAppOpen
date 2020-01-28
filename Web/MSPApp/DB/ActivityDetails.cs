using System;
using System.Collections.Generic;

namespace MSPApp.DB
{
    public partial class ActivityDetails
    {
        public ActivityDetails()
        {
            SubmissionDetail = new HashSet<SubmissionDetail>();
        }

        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string DataType { get; set; }
        public string Name { get; set; }

        public virtual ActivityType Activity { get; set; }
        public virtual ICollection<SubmissionDetail> SubmissionDetail { get; set; }
    }
}
