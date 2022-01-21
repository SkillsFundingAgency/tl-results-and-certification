using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;

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

        // Below two properties are used to render the HiddenActionText in the page.
        public ComponentType ComponentType { get; set; }
        public string HiddenActionText { get { return ComponentType == ComponentType.Core ? ResultDetailsContent.Hidden_Text_Core : ResultDetailsContent.Hidden_Text_Specialism; } }
    }
}