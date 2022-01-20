using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ComponentExamViewModel
    {
        public int AssessmentId { get; set; }
        public string AssessmentSeries { get; set; }
        public string Grade { get; set; }
        public string PrsDisplayText { get { return CommonHelper.GetPrsStatusDisplayText(PrsStatus, AppealEndDate); } }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime AppealEndDate { get; set; }
        public PrsStatus? PrsStatus { get; set; }

        public string HiddenActionText { get; set; } // TODO:
    }
}