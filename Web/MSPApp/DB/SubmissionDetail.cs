using System;
using System.Collections.Generic;

namespace MSPApp.DB
{
    public partial class SubmissionDetail
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public int ActivityDetailId { get; set; }
        public string Value { get; set; }

        public virtual ActivityDetails ActivityDetail { get; set; }
        public virtual Submission Submission { get; set; }
    }
}
