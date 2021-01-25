using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class AddCoreResultViewModel
    {
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentSeries { get; set; }
        public string PathwayDisplayName { get; set; }
        
        public string SelectedGradeCode { get; set; }
        public List<LookupDataViewModel> Grades { get; set; }
        
        public BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.ResultDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }

    public class LookupDataViewModel
    {
        // TODO: can this be a common and specific to Results? prefer to be a common
        public int Id { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
    }
}