using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class PrintBatchItem : BaseEntity
    {
        public PrintBatchItem()
        {
            PrintCertificates = new HashSet<PrintCertificate>();
        }

        public int BatchId { get; set; }
        public int TlProviderAddressId { get;set; }

        public PrintingBatchItemStatus? Status { get; set; }
        public string Reason { get; set; }
        public string TrackingId { get; set; }
        public string SignedForBy { get; set; }
        public DateTime? StatusChangedOn { get; set; }

        public virtual Batch Batch { get; set; }
        public virtual TlProviderAddress TlProviderAddress { get; set; }
        public virtual ICollection<PrintCertificate> PrintCertificates { get; set; }
    }
}