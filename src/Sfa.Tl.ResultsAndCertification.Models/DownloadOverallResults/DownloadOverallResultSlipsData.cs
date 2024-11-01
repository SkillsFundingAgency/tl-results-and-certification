using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults
{
    public class DownloadOverallResultSlipsData
    {
        [DisplayName(DownloadOverallResultSlipsHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(DownloadOverallResultSlipsHeader.LearnerName)]
        public string LearnerName { get; set; }

        [DisplayName(DownloadOverallResultSlipsHeader.ProviderName)]
        public string ProviderName { get; set; }

        [DisplayName(DownloadOverallResultSlipsHeader.ProviderUkprn)]
        public string ProviderUkprn { get; set; }

        [DisplayName(DownloadOverallResultSlipsHeader.Tlevel)]
        public string Tlevel { get { return Details.TlevelTitle; } }

        [DisplayName(DownloadOverallResultSlipsHeader.CoreComponent)]
        public string CoreComponent { get { return Details.PathwayName; } }

        [DisplayName(DownloadOverallResultSlipsHeader.CoreCode)]
        public string CoreCode { get { return Details.PathwayLarId; } }

        [DisplayName(DownloadOverallResultSlipsHeader.CoreAssessmentExamPeriod)]
        public string CoreAssessmentSeries { get; set; }

        [DisplayName(DownloadOverallResultSlipsHeader.CoreResult)]
        public string CoreResult { get { return Details.PathwayResult; } }

        [DisplayName(DownloadOverallResultSlipsHeader.SpecialismComponent)]
        public string SpecialismComponent { get; set; }

        [DisplayName(DownloadOverallResultSlipsHeader.SpecialismCode)]
        public string SpecialismCode { get; set; }

        [DisplayName(DownloadOverallResultSlipsHeader.SpecialismAssessmentExamPeriod)]
        public string SpecialismAssessmentSeries { get; set; }

        [DisplayName(DownloadOverallResultSlipsHeader.SpecialismResult)]
        public string SpecialismResult { get; set; }

        [DisplayName(DownloadOverallResultSlipsHeader.IndustryPlacementStatus)]
        public string IndustryPlacementStatus { get { return Details.IndustryPlacementStatus; } }

        [DisplayName(DownloadOverallResultSlipsHeader.OverallResult)]
        public string OverallResult { get; set; }

        public OverallResultDetail Details { get; set; }
    }
}
