using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class UlnAssessmentsNotFoundViewModel : UlnNotFoundViewModel
    {
        public override BackLinkModel BackLink => new BackLinkModel 
        { 
            RouteName = RouteConstants.SearchAssessments,
            RouteAttributes = new Dictionary<string, string> { { Constants.PopulateUln, true.ToString() } }
        };
    }
}
