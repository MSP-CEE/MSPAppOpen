using System;
using System.Collections.Generic;

namespace MSPApp.DB
{
    public partial class SubmissionAssociation
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public int UserId { get; set; }

        public virtual Submission Submission { get; set; }
        public virtual UserData User { get; set; }
    }
}
