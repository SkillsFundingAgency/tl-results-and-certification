using Sfa.Tl.ResultsAndCertification.Models.OverallResults;

namespace Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults
{
    public class DownloadOverallResultSlipsData
    {
        public long Uln { get; set; }

        public string LearnerName { get; set; }

        public string ProviderName { get; set; }

        public string ProviderUkprn { get; set; }

        public string Tlevel => Details.TlevelTitle;

        public string CoreComponent => Details.PathwayName;

        public string CoreCode => Details.PathwayLarId;

        public string CoreAssessmentSeries { get; set; }

        public string CoreResult => Details.PathwayResult;

        public string SpecialismComponent { get; set; }

        public string SpecialismCode { get; set; }

        public string SpecialismAssessmentSeries { get; set; }

        public string SpecialismResult { get; set; }

        public string IndustryPlacementStatus => Details.IndustryPlacementStatus;

        public string OverallResult { get; set; }

        public OverallResultDetail Details { get; set; }
    }
}
