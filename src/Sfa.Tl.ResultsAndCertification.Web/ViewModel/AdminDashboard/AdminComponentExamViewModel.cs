using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.Collections.Generic;
using System;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminComponentExamViewModel
    {
        public int AssessmentId { get; set; }
        public string AssessmentSeries { get; set; }
        public string Grade { get; set; }
        public string GradeCode { get; set; }
        public string PrsDisplayText { get { return CommonHelper.IsValidGradeForChangeResult(GradeCode, ComponentType) ? string.Empty : CommonHelper.GetPrsStatusDisplayText(PrsStatus, RommEndDate, AppealEndDate); } }
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime ResultEndDate { get; set; }
        public DateTime RommEndDate { get; set; }
        public DateTime AppealEndDate { get; set; }
        public PrsStatus? PrsStatus { get; set; }

        // Below properties are used to render the HiddenActionText in the page.
        public int RegistrationPathwayId { get; set; }
        public ComponentType ComponentType { get; set; }

        public bool IsResultChangeAllowed => (PrsStatus.HasValue == false || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.NotSpecified) &&
                                             ((!string.IsNullOrEmpty(GradeCode) && CommonHelper.IsValidGradeForChangeResult(GradeCode, ComponentType)) ||
                                             (!string.IsNullOrEmpty(Grade) && DateTime.Today <= ResultEndDate));

        //public string HiddenActionText { get { return ComponentType == ComponentType.Core ? ResultDetailsContent.Hidden_Text_Core : ResultDetailsContent.Hidden_Text_Specialism; } }

        //public string ResultRouteName { get { return ComponentType == ComponentType.Core ? GetPathwayResultRouteName : GetSpecialismResultRouteName; } }

        //public Dictionary<string, string> ResultRouteAttributes { get { return new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.AssessmentId, AssessmentId.ToString() } }; } }

        //private string GetPathwayResultRouteName { get { return string.IsNullOrWhiteSpace(Grade) ? RouteConstants.AddCoreResult : RouteConstants.ChangeCoreResult; } }

        //private string GetSpecialismResultRouteName { get { return string.IsNullOrWhiteSpace(Grade) ? RouteConstants.AddSpecialismResult : RouteConstants.ChangeSpecialismResult; } }
    }
}