using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSPApp.DB
{
    public partial class Submission
    {
        public Submission()
        {
            SubmissionAssociation = new HashSet<SubmissionAssociation>();
            SubmissionDetail = new HashSet<SubmissionDetail>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int ActivityId { get; set; }
        public string AnythingElse { get; set; }

        public virtual ActivityType Activity { get; set; }
        public virtual UserData User { get; set; }
        public virtual ICollection<SubmissionAssociation> SubmissionAssociation { get; set; }
        public virtual ICollection<SubmissionDetail> SubmissionDetail { get; set; }

        [NotMapped]
        public List<string> Details { get; set; }

        [NotMapped]
        public string MSPsThatHelped { get; set; }
    }
}
