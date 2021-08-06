using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum PrintingStatus
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
        [Display(Name = "Cancelled")]
        Cancelled = 6        
    }
}