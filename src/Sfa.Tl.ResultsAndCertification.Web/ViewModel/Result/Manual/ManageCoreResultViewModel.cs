using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ManageCoreResultViewModel
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentSeries { get; set; }
        public DateTime? AppealEndDate { get; set; }
        public string PathwayDisplayName { get; set; }

        public int? ResultId { get; set; }
        public string SelectedGradeCode { get; set; }
        public int? LookupId { get; set; }
        public PrsStatus? PathwayPrsStatus { get; set; }
        public List<LookupViewModel> Grades { get; set; }
        public bool IsValid => (PathwayPrsStatus.HasValue == false || PathwayPrsStatus == PrsStatus.NotSpecified) && CommonHelper.IsAppealsAllowed(AppealEndDate);

        public BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.ResultDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}