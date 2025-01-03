using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AdminLearnerRecord
    {
        public int RegistrationPathwayId { get; set; }

        public long Uln { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime DateofBirth { get; set; }

        public bool IsRegistered
            => Pathway != null && Pathway.Status == RegistrationPathwayStatus.Active || Pathway.Status == RegistrationPathwayStatus.Withdrawn;

        public bool IsPendingWithdrawl { get; set; }

        public SubjectStatus? MathsStatus { get; set; }

        public SubjectStatus? EnglishStatus { get; set; }

        public CalculationStatus? OverallCalculationStatus { get; set; }

        public Pathway Pathway { get; set; }

        public AwardingOrganisation AwardingOrganisation { get; set; }

        public string OverallResult { get; set; }

        public int? PrintCertificateId { get; set; }

        public PrintCertificateType? PrintCertificateType { get; set; }

        public DateTime? LastPrintCertificateRequestedDate { get; set; }

        public Address ProviderAddress { get; set; }

        public DateTime? PrintRequestSubmittedOn { get; set; }

        public int? BatchId { get; set; }

        public string TrackingId { get; set; }

        public PrintingBatchItemStatus? PrintingBatchItemStatus { get; set; }

        public DateTime? PrintingBatchItemStatusChangedOn { get; set; }
    }
}