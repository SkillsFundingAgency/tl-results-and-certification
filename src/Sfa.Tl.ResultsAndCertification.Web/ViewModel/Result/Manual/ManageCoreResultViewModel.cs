using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ManageCoreResultViewModel
    {
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentSeries { get; set; }
        public string PathwayDisplayName { get; set; }

        public int? ResultId { get; set; }
        public string SelectedGradeCode { get; set; }
        public int? LookupId { get; set; }
        public List<LookupViewModel> Grades { get; set; }
        
        public BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.ResultDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}