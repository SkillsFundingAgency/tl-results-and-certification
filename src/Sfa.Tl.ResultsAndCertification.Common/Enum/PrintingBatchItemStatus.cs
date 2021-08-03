using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum PrintingBatchItemStatus
    {
        NotSpecified = 0,
        [Display(Name = "Awaiting processing")]
        AwaitingProcessing = 1,
        [Display(Name = "Queued for printing")]
        QueuedForPrinting = 2,
        [Display(Name = "Printing in progress")]
        PrintingInProgress = 3,
        [Display(Name = "Awaiting collection by courier")]
        AwaitingCollectionByCourier = 4,
        [Display(Name = "Collected by courier")]
        CollectedByCourier = 5,
        [Display(Name = "Delivered")]
        Delivered = 6,
        [Display(Name = "Not Delivered")]
        NotDelivered = 7,
        [Display(Name = "Cancelled")]
        Cancelled = 8
    }
}
