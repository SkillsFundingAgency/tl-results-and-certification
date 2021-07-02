using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsPathwayGradeCheckAndSubmitViewModel : PrsBaseViewModel
    {
        public int ProfileId { get; set; }
        public int PathwayAssessmentId { get; set; }
        public int PathwayResultId { get; set; }

        public override BackLinkModel BackLink => new BackLinkModel
        {
            // TODO: conditional route 
            RouteName = RouteConstants.PrsSearchLearner,
            RouteAttributes = new Dictionary<string, string> { { Constants.PopulateUln, true.ToString() } }
        };
    }
}
