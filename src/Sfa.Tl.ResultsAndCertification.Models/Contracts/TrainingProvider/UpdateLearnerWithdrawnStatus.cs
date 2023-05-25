using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class UpdateLearnerWithdrawnStatus
    {
        public int ProfileId { get; set; }
        public bool IsPendingWithdrawl { get; set; }
    }
}
