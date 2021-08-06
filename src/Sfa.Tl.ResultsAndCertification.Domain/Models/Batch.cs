using Sfa.Tl.ResultsAndCertification.Common.Enum;
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

        public BatchType Type { get; set; }
        public BatchStatus Status { get; set; }
        public string Errors { get; set; }
        public PrintingStatus? PrintingStatus { get; set; }
        public DateTime? RunOn { get; set; }
        public DateTime? StatusChangedOn { get; set; }
        public ResponseStatus? ResponseStatus { get; set; }
        public string ResponseMessage { get; set; }

        public virtual ICollection<PrintBatchItem> PrintBatchItems { get; set; }
    }
}