using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class Batch : BaseEntity
    {
        public Batch()
        {
            PrintBatchItems = new HashSet<PrintBatchItem>();
        }

        public int Type { get; set; }
        public int Status { get; set; }
        public string Errors { get; set; }
        public DateTime? RunOn { get; set; }
        public DateTime? StatusChangedOn { get; set; }

        public virtual ICollection<PrintBatchItem> PrintBatchItems { get; set; }
    }
}