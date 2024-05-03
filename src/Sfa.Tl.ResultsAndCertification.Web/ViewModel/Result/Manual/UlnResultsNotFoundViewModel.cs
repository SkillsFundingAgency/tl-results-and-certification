using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class UlnResultsNotFoundViewModel : UlnNotFoundViewModel
    {
        public override BackLinkModel BackLink => new BackLinkModel 
        { 
            RouteName = RouteConstants.SearchResults, 
            RouteAttributes = new Dictionary<string, string> { { Constants.PopulateUln, true.ToString() } } 
        };
    }
}
