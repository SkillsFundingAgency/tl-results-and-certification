using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsUlnNotFoundViewModel
    {
        public string Uln { get; set; }

        public virtual BackLinkModel BackLink => new BackLinkModel 
        { 
            RouteName = RouteConstants.PrsSearchLearner, 
            RouteAttributes = new Dictionary<string, string> { { Constants.PopulateUln, true.ToString() } }
        };
    }
}